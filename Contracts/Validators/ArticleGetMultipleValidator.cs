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
    public class ArticleGetMultipleValidator : ValidatorInterface
    {

        private ArticleGetContract body;
        private ErrorDetails errorDetails;

        public ArticleGetMultipleValidator(ArticleGetContract body)
        {
            this.body = body;
            this.errorDetails = ErrorTypes.Incomplete;
        }

        public bool Validate()
        {
            if (this.body == null)
            {
                this.errorDetails = ErrorTypes.Err_BadRequest;
                return false;
            }

            if (this.body.Slug != null)
            {
                this.errorDetails = ErrorTypes.Err_BadRequest; // zero tolerance. To be safe. 
                return false;
            }

            if (this.body.Limit != null && this.body.Limit < 1)
            {
                this.errorDetails = ErrorTypes.Err_ArticleGet_Limit;
                return false;
            }

            if (this.body.Offset != null && this.body.Offset < 0)
            {
                this.errorDetails = ErrorTypes.Err_ArticleGet_Offset;
                return false;
            }

            // Authorname, FollowedByName...
            if (this.body.Authorname != null && this.body.Authorname.Length < 1) // TODO min username length
            {
                this.errorDetails = ErrorTypes.Err_UsernamePolicy;
                return false;
            }
            if (this.body.FollowedByName != null && this.body.FollowedByName.Length < 1) // todo min username length
            {
                this.errorDetails = ErrorTypes.Err_UsernamePolicy;
                return false;
            }

            if (this.body.Tags != null)
            {
                foreach (string tag in this.body.Tags)
                {
                    if (string.IsNullOrEmpty(tag) || tag.Length < ArticlePolicy.MIN_TAG_LENGTH || tag.Length > ArticlePolicy.MAX_TAG_LENGTH)
                    {
                        this.errorDetails = ErrorTypes.Err_ArticleTags;
                        return false;
                    }
                }
            }
            this.errorDetails = ErrorTypes.Valid;
            return true;
        }

        public ErrorDetails GetError()
        {
            return this.errorDetails;
        }
    }
}