using System.Runtime.Serialization;

namespace Contracts.Communicator.Response
{
    [DataContract]
    public class UserDetailsResponseElement
    {
        [DataMember (Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "bio")]
        public string Bio {  get; set; }

        [DataMember(Name = "image")]
        public string Image { get; set; }
    }


    [DataContract]
    public class UserDetailsResponse
    {
        public UserDetailsResponseElement User {  get; set; }
        public ErrorResponse? Error { get; set; }
    }

}
