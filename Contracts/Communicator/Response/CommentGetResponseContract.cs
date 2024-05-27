using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Communicator.Response
{
    [DataContract]
    public class CommentsGetResponse
    {
        public CommentGetResponse()
        {
            this.Comments = new List<CommentGetResponseContract>();
            this.Error = new ErrorDetails();
        }

        [DataMember]
        public ErrorDetails Error { get; set; }

        [DataMember]
        public List<CommentGetResponseContract> Comments { get; set; }
    }

    [DataContract]
    public class CommentGetResponse
    {
        public CommentGetResponse()
        {
            this.Comment = new CommentGetResponseContract();
            this.Error = new ErrorDetails();
        }

        [DataMember]
        public ErrorDetails Error { get; set; }

        [DataMember]
        public CommentGetResponseContract Comment { get; set; }
    }

    [DataContract]
    public class CommentGetResponseContract
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public DateTime UpdatedAt { get; set; }

        [DataMember]
        public string Body { get; set; } = "";

        [DataMember]
        public AuthorResponseContract Author { get; set; } = new AuthorResponseContract();
    }
}
