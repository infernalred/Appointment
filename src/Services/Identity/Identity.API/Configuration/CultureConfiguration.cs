namespace Identity.API.Configuration;

public class CultureConfiguration
{
    public static readonly string[] AvailableCultures = { "en", "fa", "fr", "ru", "sv", "zh", "es", "da", "de", "nl", "fi", "pt" };
    public static readonly string DefaultRequestCulture = "en";

    public IEnumerable<string>? Cultures { get; init; }
    public string DefaultCulture { get; set; } = DefaultRequestCulture;
}