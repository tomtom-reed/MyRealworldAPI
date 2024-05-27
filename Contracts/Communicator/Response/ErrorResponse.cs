using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Communicator.Response
{
    public static class CALLER_ERR_CD
    {
        public const int SUCCESS = 0;
        public const int GENERIC_ERROR = 1;
    }
    
    [DataContract]
    public class ErrorResponse
    {
        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; } = "";

    }
}
