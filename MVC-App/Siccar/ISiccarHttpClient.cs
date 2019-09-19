namespace MVC_App.Siccar
{
    public interface ISiccarHttpClient
    {
        string Get(string url, string idToken, bool ensureResponseIsValid = true);
    }
}