using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ListOfEmployees.Domain.Entities
{
    public class Employee
    {
        [HiddenInput(DisplayValue = false)]
        public int EmployeeId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please, enter employee name and surname!")]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Position")]
        [Required(ErrorMessage = "Please, enter employee position!")]
        [StringLength(40, MinimumLength = 2)]
        public string Position { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Please, enter employee status!")]
        public string Status { get; set; }

        [Display(Name = "Salary (UAH)")]
        [Required]
        [Range(typeof(decimal), "0,00", "10000000,00", ErrorMessage = "Please, enter employee salary!")]
        public decimal Salary { get; set; }

        [HiddenInput(DisplayValue = false)]
        public decimal Tax { get; set; }

        [HiddenInput(DisplayValue = false)]
        public decimal Net { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
