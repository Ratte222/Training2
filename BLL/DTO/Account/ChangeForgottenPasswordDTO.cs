using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTO.Account
{
    public class ChangeForgottenPasswordDTO
    {
        [Required(ErrorMessage = "Token not specified")]
        public string Token { get; set; }

        [StringLength(32, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        [Required(ErrorMessage = "New password not specified")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Entered password not correct")]
        public string ConfirmPassword { get; set; }
    }
}
