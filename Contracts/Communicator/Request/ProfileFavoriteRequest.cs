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
    public class ProfileFavoriteRequest
    {
        [DataMember]
        public ProfileFavoriteContract Contract { get; set; } = new ProfileFavoriteContract();

        [DataMember]
        public AuditContract Audit { get; set; } = new AuditContract();
    }

    [DataContract]
    public class ProfileFavoriteContract
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string ArticleSlug { get; set; } = "";
    }
}
