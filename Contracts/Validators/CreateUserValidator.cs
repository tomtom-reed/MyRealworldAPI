//using Contracts.Communicator.Request;
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
    public class CreateUserValidator : ValidatorInterface
    {
        private UserCreateContract user;
        private ErrorDetails errorDetails;
        public CreateUserValidator(UserCreateContract user) {
            this.user = user;
            this.errorDetails = ErrorTypes.Incomplete;
        }

        public bool Validate()
        {
            if (this.user == null)
            {
                this.errorDetails = ErrorTypes.Err_BadRequest;
                return false;
            }
            // Username Validation
            
            if (!UsernamePolicy.ValidateAgainstUsernamePolicy(this.user.Username)) 
            {
                this.errorDetails = ErrorTypes.Err_UsernamePolicy;
                return false;
            }

            // Email Validation
            if (!EmailPolicy.ValidateEmail(this.user.Email))
            {
                this.errorDetails = ErrorTypes.Err_EmailPolicy;
                return false;
            }
            
            // Password validation
            if (!PasswordPolicy.ValidateAgainstPasswordPolicy(this.user.Password))
            {
                this.errorDetails = ErrorTypes.Err_PasswordPolicy;
                return false;
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
