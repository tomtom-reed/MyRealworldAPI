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
    public class CommentDeleteRequest
    {
        public CommentDeleteRequest()
        {
            this.Comment = new CommentDeleteContract();
            this.Audit = new AuditContract();
        }

        [DataMember]
        public CommentDeleteContract Comment { get; set; }

        [DataMember]
        public AuditContract Audit { get; set; }
    }

    [DataContract]
    public class CommentDeleteContract
    {
        [DataMember]
        public string Slug { get; set; } = "";

        [DataMember]
        public int CommentId { get; set; }

        [DataMember]
        public int AuthorId { get; set; }
    }
}
