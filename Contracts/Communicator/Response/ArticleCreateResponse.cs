﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Communicator.Response
{
    [DataContract]
    public class ArticleCreateResponse
    {
        public ArticleCreateResponse()
        {
            this.Article = new ArticleGetResponseContract();
            this.Error = new ErrorResponse();
        }

        [DataMember]
        public ArticleGetResponseContract Article { get; set; }

        [DataMember]
        public ErrorResponse Error { get; set; }
    }
}
