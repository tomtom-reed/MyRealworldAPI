using Contracts.Communicator.Request;
using Contracts.Models;
using Contracts.Policies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Validators
{
    public class UserUpdateValidator
    {
        private readonly UserUpdateContract req;
        private ErrorDetails err;
        public UserUpdateValidator(UserUpdateContract contract)
        {
            req = contract;
            err = ErrorTypes.Incomplete;
        }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(req.UserCurrentEmail))
            {
                err = ErrorTypes.Err_BadRequest;
                return false;
            }
            // At least one updatable must be present
            if (req.Email == null 
                && req.Username == null
                && req.Password == null
                && req.Bio == null 
                && req.Image == null) {
                err = ErrorTypes.Err_BadRequest;
                return false; 
            }
            if (req.Email != null && !EmailPolicy.ValidateEmail(req.Email))
            {
                err = ErrorTypes.Err_EmailPolicy;
                return false;
            }
            if (req.Username != null && !UsernamePolicy.ValidateAgainstUsernamePolicy(req.Username))
            {
                err = ErrorTypes.Err_UsernamePolicy;
                return false;
            }
            if (req.Password != null && !PasswordPolicy.ValidateAgainstPasswordPolicy(req.Password))
            {
                err = ErrorTypes.Err_PasswordPolicy;
                return false;
            }

            // Image and Bio are allowed to be empty string

            return true;
        }

        public ErrorDetails GetError()
        {
            return err;
        }
    }
}
