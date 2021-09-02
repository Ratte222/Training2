using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO.Client
{
    public class ClientDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AdditionalInfo { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IList<string> Roles { get; set; }
    }
}
