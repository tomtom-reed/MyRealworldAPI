using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Contracts.Policies
{
    internal class PasswordPolicy
    {
        private static int minLength = 8;
        private static string regex = "^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&+=]).*$";
        public static bool ValidateAgainstPasswordPolicy(string password)
        {
            if (password == null) return false;
            if (password.Length < minLength) return false;
            if (!Regex.Match(password, regex).Success) return false;
            return true;
        }
    }
}
