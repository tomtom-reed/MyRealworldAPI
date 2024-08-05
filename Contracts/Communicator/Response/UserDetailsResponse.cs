using System.Runtime.Serialization;

namespace Contracts.Communicator.Response
{
    [DataContract]
    public class UserDetailsResponseElement
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; } = "";

        [DataMember(Name = "email")]
        public string Email { get; set; } = "";

        [DataMember(Name = "bio")]
        public string Bio { get; set; } = "";

        [DataMember(Name = "image")]
        public string Image { get; set; } = "";
    }


    [DataContract]
    public class UserDetailsResponse
    {
        public UserDetailsResponse()
        {
            User = new UserDetailsResponseElement();
        }

        [DataMember(Name = "user")]
        public UserDetailsResponseElement User {  get; set; }

        [DataMember(Name = "error")]
        public ErrorResponse? Error { get; set; }
    }

}
