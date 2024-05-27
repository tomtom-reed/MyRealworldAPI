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
    public class CommentCreateValidator : ValidatorInterface
    {
        private CommentCreateContract comment;
        private ErrorDetails error;

        public CommentCreateValidator(CommentCreateContract comment)
        {
            this.comment = comment;
            this.error = ErrorTypes.Incomplete;
        }

        public bool Validate()
        {
            if (this.comment == null)
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }

            if (this.comment.ArticleSlug == null || this.comment.ArticleSlug.Length != ArticlePolicy.SLUG_LENGTH)
            {
                this.error = ErrorTypes.Err_ArticleSlug;
                return false;
            }
            if (this.comment.AuthorId <= 0)
            {
                this.error = ErrorTypes.Err_CommentAuthorId;
                return false;
            }
            if (this.comment.Body == null || this.comment.Body.Length > CommentPolicy.MAX_BODY_LENGTH)
            {
                this.error = ErrorTypes.Err_CommentBody;
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
