using System.Threading.Tasks;

namespace Siccar.Connector.STS
{
    public interface ISiccarSTSClient
    {
        Task<string> ExtendTokenAttestation(string url, string token, string attestations);
        Task<string> ExtendTokenClaims(string url, string token, string claims);

    }
}