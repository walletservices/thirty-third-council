
namespace MVC_App.Siccar
{
    public interface ISiccarConfig
    {
        string ProcessA { get; set; }
        string ProcessB { get; set; }
        string ProcessC { get; set; }
        string ExpectedClaims { get; set; }
        string ExpectedAttestations { get; set; }

        string SiccarSTSClientId { get; set; }
        string RegisterId { get; set; }
    }
}
