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
    public class ArticleUpdateRequest
    {
        public ArticleUpdateRequest()
        {
            this.Article = new ArticleUpdateContract();
            this.Audit = new AuditContract();
        }

        [DataMember]
        public ArticleUpdateContract Article { get; set; }

        [DataMember]
        public AuditContract Audit { get; set; }
    }

    [DataContract]
    public class ArticleUpdateContract
    {
        public ArticleUpdateContract()
        {
            this.Tags = new List<string>();
        }

        [DataMember]
        public string Slug { get; set; } = "";

        [DataMember]
        public int AuthorId { get; set; }

        [DataMember]
        public string? Title { get; set; }

        [DataMember]
        public string? Description { get; set; }

        [DataMember]
        public string? Body { get; set; }

        [DataMember]
        public List<string>? Tags { get; set; }
    }
}
