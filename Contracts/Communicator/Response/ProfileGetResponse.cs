using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Communicator.Response
{
    [DataContract]
    public class ProfileGetResponse
    {
        [DataMember]
        public ProfileGetResponseContract Profile { get; set; }

        [DataMember]
        public ErrorDetails Error { get; set; }

        public ProfileGetResponse()
        {
            Profile = new ProfileGetResponseContract();
            Error = new ErrorDetails();
        }
    }

    [DataContract]
    public class ProfileGetResponseContract
    {
        [DataMember]
        public string Username { get; set; } = "";

        [DataMember]
        public string Bio { get; set; } = "";

        [DataMember]
        public string Image { get; set; } = "";

        [DataMember]
        public bool? Following { get; set; }
    }
}
