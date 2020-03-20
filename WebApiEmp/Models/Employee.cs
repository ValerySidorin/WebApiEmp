using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiEmp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; }

        [Range(1, 100, ErrorMessage = "Неверный возраст")]
        [Required(ErrorMessage = "Укажите возраст")]
        public int Age { get; set; }
    }
}
