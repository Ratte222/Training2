using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTO.Account
{
    public class LoginModelDTO
    {
        [Required(ErrorMessage = "UserName not specified")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
