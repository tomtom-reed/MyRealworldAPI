using Contracts.Models;
using Contracts.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Validators
{
    public class UserLoginValidator
    {
        private readonly LoginContract req;
        private ErrorDetails err;
        public UserLoginValidator(LoginContract req) {
            this.req = req;
            this.err = ErrorTypes.Incomplete;
        }

        public bool Validate()
        {
            if (!EmailPolicy.ValidateEmail(this.req.Email))
            {
                return false;
            }
            if (!PasswordPolicy.ValidateAgainstPasswordPolicy(this.req.Password))
            {
                // Save just a few ms. Slightly problematic if the password policy changes. 
                return false;
            }
            return true;
        }
        public ErrorDetails GetError()
        {
            return err;
        }

    }
}
