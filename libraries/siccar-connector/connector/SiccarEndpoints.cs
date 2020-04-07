
namespace Siccar.Connector.Connector
{
    public class SiccarEndpoints : ISiccarEndpoints
    {
        public string GetGetActionToLoad { get; set; }
        public string GetCheckifActionIsStartable { get; set; }
        public string GetProcessesThatICanStart { get; set; }
        public string PostStartProcess { get; set; }
        public string PostSubmitStep { get; set; }
        public string GetGetProgressReports { get; set; }
        public string TokenEndpoint { get; set; }
        public string ExpectedClaims { get; set; }
        public string ExpectedAttestations { get; set; }

        public string GetTransaction { get; set; }
        public string GetDocumentTransaction { get; set; }
        public string GetMe { get; set; }
        public string GetStartableProcesses { get; set; }

    }
}
