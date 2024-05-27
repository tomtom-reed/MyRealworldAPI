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
    public class CommentDeleteValidator : ValidatorInterface
    {
        private CommentDeleteContract comment;
        private ErrorDetails error;
        public CommentDeleteValidator(CommentDeleteContract comment)
        {
            this.comment = comment;
            this.error = ErrorTypes.Incomplete;
        }

        public bool Validate()
        {
            if (comment.Equals(null))
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }
            if (comment.AuthorId <= 0)
            {
                this.error = ErrorTypes.Err_CommentAuthorId;
                return false;
            }
            if (comment.CommentId <= 0)
            {
                this.error = ErrorTypes.Err_CommentId;
                return false;
            }
            if (comment.Slug == null || comment.Slug.Length != ArticlePolicy.SLUG_LENGTH)
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
