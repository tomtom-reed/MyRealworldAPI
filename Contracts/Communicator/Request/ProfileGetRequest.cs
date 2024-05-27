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
    public class ProfileGetRequest
    {
        [DataMember]
        public ProfileGetContract Profile { get; set; } = new ProfileGetContract();

        [DataMember]
        public AuditContract Audit { get; set; } = new AuditContract();
    }

    [DataContract]
    public class ProfileGetContract
    {
        [DataMember]
        public int? UserId { get; set; }

        [DataMember]
        public string FollowedUsername { get; set; } = "";
    }
}
