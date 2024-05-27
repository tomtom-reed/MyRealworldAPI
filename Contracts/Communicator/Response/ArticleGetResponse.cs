using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Communicator.Response
{
    [DataContract]
    public class ArticleGetResponse
    {
        public ArticleGetResponse()
        {
            this.Article = new ArticleGetResponseContract();
            this.Error = new ErrorResponse();
        }

        [DataMember]
        public ArticleGetResponseContract Article { get; set; }

        [DataMember]
        public ErrorResponse Error { get; set; }
    }

    [DataContract]
    public class ArticleGetResponseContract
    {
        [DataMember]
        public string Slug { get; set; } = "";

        [DataMember]
        public string Title { get; set; } = "";

        [DataMember]
        public string Description { get; set; } = "";

        [DataMember]
        public string Body { get; set; } = "";

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public DateTime UpdatedAt { get; set; }

        [DataMember]
        public List<string> Tags { get; set; } = new List<string>();

        [DataMember]
        public bool Favorited { get; set; }

        [DataMember]
        public int FavoritesCount { get; set; }

        [DataMember]
        public AuthorResponseContract Author { get; set; } = new AuthorResponseContract();
    }

    [DataContract]
    public class AuthorResponseContract {

        [DataMember]
        public string Username { get; set; } = "";

        [DataMember]
        public string Bio { get; set; } = "";

        [DataMember]
        public string Image { get; set; } = "";

        [DataMember]
        public bool Following { get; set; }
    }
}
