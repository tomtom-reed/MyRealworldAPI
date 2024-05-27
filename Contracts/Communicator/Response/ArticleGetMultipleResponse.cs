using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Communicator.Response
{
    [DataContract]
    public class ArticleGetMultipleResponse
    {
        public ArticleGetMultipleResponse()
        {
            this.Articles = new List<ArticleGetResponseContract>();
            this.Error = new ErrorResponse();
        }
        public List<ArticleGetResponseContract> Articles { get; set; }
        public ErrorResponse Error { get; set; }
    }
}
