using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Model
{
    public class Client: IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? RemoveData { get; set; }

    }

    public class AccountRole
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Guest = "Guest";
    }
}
