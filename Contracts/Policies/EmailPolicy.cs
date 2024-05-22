using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Policies
{
    internal class EmailPolicy
    {
        // System.Net.Mail.MailAddress verified emails against probably RFC 2822, maybe 5322. Documentation doesn't say. 
        // That means that "a@a" is technically a valid email using this code. 
        // It would be possibly to modify this block to have more rules like requiring valid domains or blocking regions. 
        public static bool ValidateEmail(string email)
        {
            if (email == null)
            {
                return false;
            }
            else
            {
                try
                {
                    String trimmedEmail = email.Trim();
                    MailAddress mailAddress = new System.Net.Mail.MailAddress(email);
                    if (trimmedEmail != mailAddress.Address)
                    {
                        return false;
                    }
                    // Possible: add blocked domains etc here
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    return false;
                }
            }
            return true;
        }
    }
}
