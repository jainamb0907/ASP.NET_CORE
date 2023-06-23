namespace Practical19_MVC.ViewModels;
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
