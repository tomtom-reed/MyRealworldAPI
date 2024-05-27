using Contracts.Communicator.Request;
using Contracts.Models;
using Contracts.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Validators
{
    public class ArticleGetOneValidator : ValidatorInterface
    {
        private ErrorDetails error;
        private ArticleGetContract body;

        public ArticleGetOneValidator(ArticleGetContract body)
        {
            this.error = ErrorTypes.Incomplete;
            this.body = body;
        }

        public bool Validate()
        {
            if (this.body == null)
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }

            if (this.body.Slug == null || this.body.Slug.Length != ArticlePolicy.SLUG_LENGTH)
            {
                this.error = ErrorTypes.Err_ArticleSlug;
                return false;
            }

            this.error = ErrorTypes.Valid;
            return true;
        }

        public ErrorDetails GetError()
        {
            return this.error;
        }
    }


}
