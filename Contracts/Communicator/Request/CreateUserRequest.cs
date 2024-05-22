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
    public class CreateUserRequest
    {
        public CreateUserRequest()
        {
            this.User = new UserCreateContract();
            this.Audit = new AuditContract();
        }
        [DataMember (Name = "user")]
        public UserCreateContract User;

        [DataMember (Name="audit")]
        public AuditContract Audit {  get; set; }
    }
}
