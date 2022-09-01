using System.Threading.Tasks;

public class InMemoryAnchorService : IAnchorService
{
    private AnchorInfo? _latestAnchorInfo;

    public InMemoryAnchorService()
    {
        _latestAnchorInfo = null;
    }
    public Task CreateAnchor(AnchorInfo anchorInfo)
    {
        _latestAnchorInfo = anchorInfo;
        return Task.CompletedTask;
    }

    public Task<AnchorInfo?> TryGetLatestAnchor()
    {
        return Task.FromResult(_latestAnchorInfo);
    }
}