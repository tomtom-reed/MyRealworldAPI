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
    public class ArticleDeleteValidator
    {
        private ErrorDetails error;
        private ArticleDeleteContract contract;

        public ArticleDeleteValidator(ArticleDeleteContract contract)
        {
            this.error = ErrorTypes.Incomplete;
            this.contract = contract;
        }

        public bool Validate()
        {
            if (this.contract == null)
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }

            if (this.contract.AuthorId <= 0)
            {
                this.error = ErrorTypes.Err_ArticleAuthorId;
                return false;
            }

            if (this.contract.Slug == null || this.contract.Slug.Length != ArticlePolicy.SLUG_LENGTH)
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
