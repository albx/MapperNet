using MapperNet.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MapperNet.WebDemo.Models.Mysql
{
    [Table(Name="people")]
    public class Person
    {
        [Column(Name="id", IsPrimaryKey=true)]
        public int Id { get; set; }

        [Column(Name="first_name")]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The first name is required")]
        public string FirstName { get; set; }

        [Column(Name="last_name")]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The last name is required")]
        public string LastName { get; set; }

        [Column(Name="date_of_birth")]
        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = "The date of birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Column(Name="age")]
        [Display(Name = "Age")]
        public int Age { get; set; }
    }
}