namespace Identity.API.Models.AccountViewModel;

public class LogoutViewModel: LogoutInputModel
{
    public bool ShowLogoutPrompt { get; set; } = true;
}