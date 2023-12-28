using CollegeApp.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Model
{
    public class StudentDTO
    {
        //Model Validation
        //To work the model validation we need to add [ApiController] into Controller class

        //Built in attribute validations
        /*
         * If we do not want to show the system generated erro message and want to show your own error msg we can use this
                eg: [Required (ErrorMessage ="Student Name is required")]
          */
        [ValidateNever] // when we do not need validation for a field we can add this
        public int Id { get; set; }

        [Required (ErrorMessage ="Student Name is required")]
        [StringLength (30)] // validating length of string

        //Auto mapper with different property names, ignore, transform
        // Changed the property name from "StudentName" to "Name".
        // Check the StudentDTO.cs and Student.cs there is the different with property name 
        public string Name { get; set; }  

        [EmailAddress(ErrorMessage = "Invalid E-mail address")]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        #region 2. Validating Date and Time Using Custom attribute
        /*
        [DateCheck] 
        public DateTime AdmissionDate { get; set; }
        */
        #endregion

        /*
        [Required]
        [Range(10,30)]
        public int Age { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))] // It will compare the ConfirmPassword with Password
        public string ConfirmPassword { get; set; }
        */
    }
}
