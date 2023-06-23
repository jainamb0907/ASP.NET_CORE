namespace Practical_17_Core.Models
{
    public class UserClaim
    {
        public string? ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }

    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
        public string? UserId { get; set; }
        public List<UserClaim>? Claims { get; set; }
    }


}
