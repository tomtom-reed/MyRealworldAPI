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
        [DataMember]
        public ErrorResponse Error {  get; set; }
    }
}
