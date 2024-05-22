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
    public class UserCreateRequest
    {
        public UserCreateRequest()
        {
            this.User = new UserCreateContract();
            this.Audit = new AuditContract();
        }

        [DataMember(Name = "userDetails")]
        public UserCreateContract User { get; set; }

        [DataMember(Name = "audit")]
        public AuditContract Audit { get; set; }
    }

    [DataContract]
    public class UserCreateContract
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}
