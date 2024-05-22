using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Communicator.Request
{
    [DataContract]
    public class LoginRequest
    {
        public LoginRequest()
        {
            Login = new LoginContract();
            Audit = new AuditContract();
        }

        [DataMember (Name = "login")]
        public LoginContract Login {  get; set; }

        [DataMember (Name = "audit")]
        public AuditContract Audit { get; set; }
    }
}
