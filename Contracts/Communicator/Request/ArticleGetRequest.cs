using Contracts.Models;
using System.Runtime.Serialization;

namespace Contracts.Communicator.Request
{

    [DataContract]
    public class ArticleGetRequest
    {
        public ArticleGetRequest()
        {
            this.Article = new ArticleGetContract();
            this.Audit = new AuditContract();
        }

        [DataMember]
        public ArticleGetContract Article { get; set; }

        [DataMember]
        public AuditContract Audit { get; set; }
    }
    [DataContract]
    public class ArticleGetContract
    {
        [DataMember]
        public string? Slug { get; set; }

        [DataMember]
        public string? Authorname { get; set; }

        [DataMember]
        public string? FollowedByName { get; set; }

        [DataMember]
        public int? FollowedById { get; set; }

        [DataMember]
        public int? Offset { get; set; }

        [DataMember]
        public int? Limit { get; set; }

        [DataMember]
        public List<string>? Tags { get; set; }
    }
}
