using System.Threading.Tasks;

public class AzureServerlessAnchorService : IAnchorService
{
    public Task CreateAnchor(AnchorInfo anchorInfo)
    {
        throw new System.NotImplementedException();
    }

    public Task<AnchorInfo?> TryGetLatestAnchor()
    {
        throw new System.NotImplementedException();
    }
}