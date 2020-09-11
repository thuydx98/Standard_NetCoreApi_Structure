using System;
using System.Collections.Generic;
using System.Text;

namespace StandardApi.Core.Auth.ViewModels
{
    public class UserTokenInfoModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string FullName { get { return (FirstName + " " + MiddleName + " " + LastName).Replace("  ", " "); } }
        public string UserName { get; set; }
        public DateTime? Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
