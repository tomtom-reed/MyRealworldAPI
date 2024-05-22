using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Contracts.Models
{
    public class ErrorDetails
    {
        public ErrorDetails() { }
        public ErrorDetails(int id,  string message)
        {
            this.ID = id;
            this.Message = message;
        }
        public int ID { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("class ErrorDetails: ")
                .Append(ID)
                .Append(" : ")
                .Append(Message)
                .ToString();
        }
    }

    public static class ErrorTypes
    {
        const int ErrorID_Valid = 0;
        const int ErrorID_Invalid = 1;

        public static readonly ErrorDetails Incomplete = new ErrorDetails(-1, "Validation not run yet");

        public static readonly ErrorDetails Valid = new ErrorDetails(0, "Valid");
        public static readonly ErrorDetails Err_BadRequest = new ErrorDetails(3, "Bad Request");

        public static readonly ErrorDetails Err_UsernameMissing = new ErrorDetails(301, "Username is required");
        public static readonly ErrorDetails Err_UsernamePolicy = new ErrorDetails(302, "Username does not fit policy");

        public static readonly ErrorDetails Err_EmailMissing = new ErrorDetails(311, "Email is required");
        public static readonly ErrorDetails Err_EmailFormat = new ErrorDetails(312, "Email is improperly formatted");
        public static readonly ErrorDetails Err_EmailPolicy = new ErrorDetails(313, "Email does not fit policy");

        public static readonly ErrorDetails Err_PasswordMissing = new ErrorDetails(321, "Password is required");
        public static readonly ErrorDetails Err_PasswordPolicy = new ErrorDetails(322, "Password does not fit policy");
    }
}
