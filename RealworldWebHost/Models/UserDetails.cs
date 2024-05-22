namespace RealworldWebHost.Models
{
    public class UserDetails
    {
        public string ErrorMsg { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public string? Bio { get; set; }
        public string? Img { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
