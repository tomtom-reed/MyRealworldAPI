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
    public class UserUpdateRequest
    {
        public UserUpdateRequest()
        {
            this.User = new UserUpdateContract();
            this.Audit = new AuditContract();
        }

        [DataMember(Name = "userDetails")]
        public UserUpdateContract User { get; set; }

        [DataMember(Name = "audit")]
        public AuditContract Audit { get; set; }
    }

    [DataContract]
    public class UserUpdateContract
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string? Username { get; set; }

        [DataMember]
        public string? Email { get; set; }

        [DataMember]
        public string? Password { get; set; }

        [DataMember]
        public string? Bio {  get; set; }

        [DataMember]
        public string? Image {  get; set; }
    }
}
