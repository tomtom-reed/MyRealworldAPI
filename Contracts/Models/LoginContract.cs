using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models
{
    [DataContract]
    public class LoginContract
    {
        [DataMember (Name = "email")]
        public string Email { get; set; }

        [DataMember (Name = "password")]
        public string Password { get; set; }
    }
}
