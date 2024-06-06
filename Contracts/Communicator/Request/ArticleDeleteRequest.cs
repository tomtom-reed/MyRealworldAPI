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
    public class ArticleDeleteRequest
    {
        public ArticleDeleteRequest()
        {
            this.Contract = new ArticleDeleteContract();
            this.Audit = new AuditContract();
        }

        [DataMember]
        public AuditContract Audit { get; set; }

        [DataMember]
        public ArticleDeleteContract Contract { get; set; }
    }

    [DataContract]
    public class ArticleDeleteContract
    {
        [DataMember]
        public string Slug { get; set; } = "";

        [DataMember]
        public int AuthorId { get; set; }
    }
}
