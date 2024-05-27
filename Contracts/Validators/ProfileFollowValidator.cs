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
    public class ProfileFollowValidator : ValidatorInterface
    {
        private ProfileFollowContract contract;
        private ErrorDetails error;

        public ProfileFollowValidator(ProfileFollowContract contract)
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
            if (!UsernamePolicy.ValidateAgainstUsernamePolicy(this.contract.FollowedUsername))
            {
                this.error = ErrorTypes.Err_UsernamePolicy;
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
