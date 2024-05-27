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
    public class ProfileFavoriteValidator : ValidatorInterface
    {
        private ProfileFavoriteContract contract;
        private ErrorDetails error;

        public ProfileFavoriteValidator(ProfileFavoriteContract contract)
        {
            this.contract = contract;
            this.error = ErrorTypes.Incomplete;
        }

        public bool Validate()
        {
            if (this.contract == null)
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }
            if (this.contract.UserId <= 0)
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }
            if (this.contract.ArticleSlug == null || this.contract.ArticleSlug.Length != ArticlePolicy.SLUG_LENGTH)
            {
                this.error = ErrorTypes.Err_ArticleSlug;
                return false;
            }
            this.error = ErrorTypes.Valid;
            return true;
        }

        public ErrorDetails GetError()
        {
            return error;
        }
    }
}
