//using Contracts.Models;
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
    public class UserDetailsRequest
    {
        public UserDetailsRequest()
        {
            UserDetails = new UserDetailsContract();
            Audit = new AuditContract();
        }
        [DataMember(Name = "userDetails")]
        public UserDetailsContract UserDetails { get; set; }

        [DataMember(Name = "audit")]
        public Contracts.Models.AuditContract Audit { get; set; }
    }

    [DataContract]
    public class UserDetailsContract
    {
        [DataMember]
        public string Email { get; set; }
    }
}
