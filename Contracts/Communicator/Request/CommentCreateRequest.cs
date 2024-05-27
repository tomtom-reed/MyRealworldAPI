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
    public class CommentCreateRequest
    {
        public CommentCreateRequest()
        {
            this.Comment = new CommentCreateContract();
            this.Audit = new AuditContract();
        }

        [DataMember]
        public CommentCreateContract Comment { get; set; }

        [DataMember]
        public AuditContract Audit { get; set; }
    }

    [DataContract]
    public class CommentCreateContract
    {
        [DataMember]
        public string ArticleSlug { get; set; } = "";

        [DataMember]
        public int AuthorId { get; set; }

        [DataMember]
        public string Body { get; set; } = "";
    }
}
