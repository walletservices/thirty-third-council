
namespace MVC_App.Siccar
{
    public class SiccarConfig : ISiccarConfig
    {
        public string ProcessA { get; set; }
        public string ProcessB { get; set; }
        public string ProcessC { get; set; }
        public string ExpectedClaims { get; set; }
        public string ExpectedAttestations { get; set; }

        public string SiccarSTSClientId { get; set; }
        public string RegisterId { get; set; }

    }
}
