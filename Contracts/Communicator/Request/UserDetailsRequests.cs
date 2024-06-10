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
    public class UserDetailsByEmailRequest
    {
        public UserDetailsByEmailRequest()
        {
            Audit = new AuditContract();
        }
        [DataMember(Name = "email")]
        public string Email { set; get; } = "";

        [DataMember(Name = "audit")]
        public Contracts.Models.AuditContract Audit { get; set; }
    }


    [DataContract]
    public class UserDetailsByUsernameRequest
    {
        public UserDetailsByUsernameRequest()
        {
            Audit = new AuditContract();
        }
        [DataMember(Name = "username")]
        public string Username { set; get; } = "";

        [DataMember(Name = "audit")]
        public Contracts.Models.AuditContract Audit { get; set; }
    }


    [DataContract]
    public class UserDetailsByIdRequest
    {
        public UserDetailsByIdRequest()
        {
            Audit = new AuditContract();
        }
        [DataMember(Name = "id")]
        public int Id { set; get; }

        [DataMember(Name = "audit")]
        public Contracts.Models.AuditContract Audit { get; set; }
    }
}
