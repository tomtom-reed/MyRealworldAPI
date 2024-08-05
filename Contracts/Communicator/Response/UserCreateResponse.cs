using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Communicator.Response
{
    [DataContract]
    public class UserCreateResponse
    {
        public UserCreateResponse()
        {
            Error = new ErrorResponse();
        }

        public UserCreateResponse(int errCd, string errMsg)
        {
            Error = new ErrorResponse();
            Error.ErrorCode = errCd;
            Error.ErrorMessage = errMsg;
        }

        [DataMember]
        public ErrorResponse Error {  get; set; }
    }
}
