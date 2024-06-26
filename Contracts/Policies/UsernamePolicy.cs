using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Policies
{
    // This class currently only forces minLength of 3 and maxLength of 40
    // It would be possible to also add moderation tooling here, but that would prevent even logins. 
    // This class is therefore mainly just to improve performance. 
    // 
    // Note that usernames must be unique. Enforcing that here is not in scope. 
    internal class UsernamePolicy
    {
        private static int minLength = 3;
        private static int maxLength = 50;

        public static bool ValidateAgainstUsernamePolicy(string username)
        {
            if (username == null) {
                return false;
            }
            if (username.Length < UsernamePolicy.minLength) { return false; }
            if (username.Length > UsernamePolicy.maxLength) { return false; }
            return true;
        }
    }
}
