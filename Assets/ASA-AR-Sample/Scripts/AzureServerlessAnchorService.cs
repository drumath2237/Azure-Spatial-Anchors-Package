using System.Threading.Tasks;

public class AzureServerlessAnchorService : IAnchorService
{
    readonly string baseUrl;

    public AzureServerlessAnchorService(string apiBaseUrl = "https://shoten13-anchorinfo.azurewebsites.net")
    {
        baseUrl = apiBaseUrl;
    }

    public Task CreateAnchor(AnchorInfo anchorInfo)
    {
        throw new System.NotImplementedException();
    }

    public Task<AnchorInfo?> TryGetLatestAnchor()
    {
        throw new System.NotImplementedException();
    }
}