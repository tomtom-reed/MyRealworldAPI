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
    public class CommentGetRequest
    {
        public CommentGetRequest()
        {
            this.Comment = new CommentGetContract();
            this.Audit = new AuditContract();
        }

        [DataMember]
        public CommentGetContract Comment { get; set; }

        [DataMember]
        public AuditContract Audit { get; set; }
    }

    [DataContract]
    public class CommentGetContract
    {
        [DataMember]
        public string Slug { get; set; } = "";

        [DataMember]
        public int? CommentId { get; set; }

        [DataMember]
        public int? FollowerId { get; set; }
    }
}
