namespace Identity.API.Models.AccountViewModel;

public class LoggedOutViewModel
{
    public string PostLogoutRedirectUri { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string SignOutIframeUrl { get; set; } = null!;

    public bool AutomaticRedirectAfterSignOut { get; set; }

    public string? LogoutId { get; set; }
    public bool TriggerExternalSignOut => !string.IsNullOrEmpty(ExternalAuthenticationScheme);
    public string ExternalAuthenticationScheme { get; set; } = string.Empty;
}