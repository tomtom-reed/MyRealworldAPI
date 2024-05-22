using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models
{
    [DataContract]
    public class UserContract
    {
        [DataMember(Name="email")]
        public string? Email { get; set; }

        [DataMember(Name="password")]
        public string? Password { get; set; }

        [DataMember(Name = "username")]
        public string? Username { get; set; }

        [DataMember(Name = "bio")]
        public string? Bio { get; set;}

        [DataMember(Name = "image")]
        public string? Image { get; set; }
    }
}
