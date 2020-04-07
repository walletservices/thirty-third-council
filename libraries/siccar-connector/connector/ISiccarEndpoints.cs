
namespace Siccar.Connector.Connector
{
    public interface ISiccarEndpoints
    {
        string GetStartableProcesses { get; set; }
        string GetGetActionToLoad { get; set; }
        string GetCheckifActionIsStartable { get; set; }
        string GetProcessesThatICanStart { get; set; }
        string PostStartProcess { get; set; }
        string PostSubmitStep { get; set; }
        string GetGetProgressReports { get; set; }
        string TokenEndpoint { get; set; }
        string ExpectedClaims { get; set; }
        string ExpectedAttestations { get; set; }
        string GetTransaction { get; set; }
        string GetDocumentTransaction { get; set; }
        string GetMe { get; set; }

    }
}
