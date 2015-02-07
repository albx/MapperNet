using MapperNet.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MapperNet.WebDemo.Models.SqlServer
{
    [Table(Name="People")]
    public class Person
    {
        [Column(IsPrimaryKey=true)]
        public int Id { get; set; }

        [Column]
        [Display(Name = "First Name")]
        [Required(ErrorMessage="The first name is required")]
        public string FirstName { get; set; }

        [Column]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage="The last name is required")]
        public string LastName { get; set; }

        [Column]
        [Display(Name = "Date of birth")]
        [Required(ErrorMessage="The date of birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Column]
        [Display(Name = "Age")]
        public int Age { get; set; }
    }
}