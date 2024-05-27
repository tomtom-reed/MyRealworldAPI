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
    public class ArticleCreateRequest
    {
        public ArticleCreateRequest() { 
            this.Article = new ArticleCreateContract();
            this.Audit = new AuditContract();
        }

        [DataMember]
        public ArticleCreateContract Article { get; set; }

        [DataMember]
        public AuditContract Audit { get; set; }

    }

    [DataContract]
    public class ArticleCreateContract
    {
        public ArticleCreateContract() { 
            this.Tags = new List<string>();
        }

        [DataMember]
        public int AuthorId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public List<string> Tags { get; set; }
    }
}
