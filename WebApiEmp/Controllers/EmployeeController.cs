using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiEmp.Models;
using WebApiEmp.Services;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace WebApiEmp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "Caching")]
    public class EmployeeController : ControllerBase
    {
        List<Employee> employees;
        EmployeeContext db;
        LogContext logdb;
        IMemoryCache cache;

        ILoggerFactory loggerFactory = LoggerFactory.Create(options =>
        {
            options.AddConsole();
        });
        ILogger logger;

        public EmployeeController(EmployeeContext context, LogContext logContext, IMemoryCache memoryCache)
        {
            employees = new List<Employee>();
            cache = memoryCache;
            db = context;
            logdb = logContext;
            loggerFactory.AddFile("logger.txt");
            logger = loggerFactory.CreateLogger<EmployeeController>();
        }

        //GET employee/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            await Log();

            if (employees.Count == 0)
            {
                foreach (Employee employee in db.Employees)
                {
                    Employee e;
                    if (!cache.TryGetValue(employee.Id, out e))
                    {
                        cache.Set(employee.Id, employee, TimeSpan.FromMinutes(5));
                    }
                    employees.Add(employee);
                }
            }
            return employees;
        }
        
        //GET employee/5/
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(int? id)
        {
            await Log();

            Employee emp;
            if (!cache.TryGetValue(id, out emp))
            {
                emp = await db.Employees.FindAsync(id);
                if (emp == null)
                    return NotFound();
                cache.Set(emp.Id, emp, TimeSpan.FromMinutes(5));
            }
            return new ObjectResult(emp);
        }

        //POST employee/
        [HttpPost]
        public async Task<IActionResult> Post(Employee emp)
        {
            string body = $"firstname: {emp.FirstName}, lastname: {emp.LastName}, age: {emp.Age}";
            await Log(body);

            if (emp.FirstName == "admin") ModelState.AddModelError("FirstName", "Bad name");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (emp == null)
                return BadRequest();

            db.Employees.Add(emp);
            int dbOperations = await db.SaveChangesAsync();
            if (dbOperations > 0)
                cache.Set(emp.Id, emp, TimeSpan.FromMinutes(5));

            return Ok(emp);
        }

        //PUT employee/
        [HttpPut]
        public async Task<ActionResult<Employee>> Put(Employee emp)
        {
            await Log();

            if (emp == null)
                return BadRequest();

            if (!db.Employees.Any(e => e.Id == emp.Id))
                return NotFound();

            db.Employees.Update(emp);
            int dbOperations = await db.SaveChangesAsync();
            if (dbOperations > 0)
                cache.Set(emp.Id, emp, TimeSpan.FromMinutes(5));

            return Ok(emp);
        }

        //DELETE employee/5/
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> Delete(int? id)
        {
            await Log();

            Employee emp = new Employee { Id = id.Value };
            db.Entry(emp).State = EntityState.Deleted;

            int dbOperations = await db.SaveChangesAsync();
            if (dbOperations > 0)
                cache.Remove(emp.Id);

            return NoContent();
        }

        private async Task Log()
        {
            Log log = new Log { DateTime = DateTime.Now.ToString(), Method = Request.Method, Protocol = Request.Protocol, Host = Request.Host.ToString(), Path = Request.Path };
            await logdb.Logs.AddAsync(log);
            await logdb.SaveChangesAsync();
            logger.LogInformation($"DateTime: {DateTime.Now.ToString()} Method: {Request.Method} Protocol: {Request.Protocol} Host: {Request.Host} Path: {Request.Path}");
        }

        private async Task Log(string body)
        {
            Log log = new Log { DateTime = DateTime.Now.ToString(), Method = Request.Method, Protocol = Request.Protocol, Host = Request.Host.ToString(), Path = Request.Path, Body = body };
            await logdb.Logs.AddAsync(log);
            await logdb.SaveChangesAsync();
            logger.LogInformation($"DateTime: {DateTime.Now.ToString()} Method: {Request.Method} Protocol: {Request.Protocol} Host: {Request.Host} Path: {Request.Path} Body: {body}");
        }
    }
}