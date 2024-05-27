using System.Runtime.Serialization;

namespace RealworldWebHost.Models
{
    [DataContract]
    public class UserDetails
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public byte[] PasswordHash { get; set; }

        [DataMember]
        public string? Bio { get; set; }

        [DataMember]
        public string? Img { get; set; }

        [DataMember]
        public DateTime? CreatedAt { get; set; }

        [DataMember]
        public DateTime? UpdatedAt { get; set; }

        [DataMember]
    }
}
