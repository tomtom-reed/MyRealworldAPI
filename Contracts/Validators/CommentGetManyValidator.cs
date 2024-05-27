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
    public class CommentGetManyValidator : ValidatorInterface
    {

        private CommentGetContract comment;
        private ErrorDetails error;

        public CommentGetManyValidator(CommentGetContract comment)
        {
            this.comment = comment;
            this.error = ErrorTypes.Incomplete;
        }

        public bool Validate()
        {
            if (this.comment.Equals(null))
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }
            if (this.comment.Slug == null || this.comment.Slug.Length != ArticlePolicy.SLUG_LENGTH)
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }
            if (this.comment.CommentId != null) // Explicit deny, no confusion
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }
            if (this.comment.FollowerId != null && this.comment.FollowerId < 0)
            {
                this.error = ErrorTypes.Err_BadRequest;
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
