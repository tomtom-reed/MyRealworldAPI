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
    public class ProfileFollowRequest
    {
        [DataMember]
        public ProfileFollowContract Contract { get; set; } = new ProfileFollowContract();

        [DataMember]
        public AuditContract Audit { get; set; } = new AuditContract();
    }

    [DataContract]
    public class ProfileFollowContract
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string FollowedUsername { get; set; } = "";
    }
}
