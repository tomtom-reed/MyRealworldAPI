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
    public class ArticleDeleteResponse
    {
        public ArticleDeleteResponse()
        {
            this.Error = new ErrorResponse();
        }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public ErrorResponse Error { get; set; }
    }
}
