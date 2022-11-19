using System.Threading.Tasks;

namespace AzureSpatialAnchorsPackage
{
    public class InMemoryAnchorService : IAnchorService
    {
        private AnchorInfo? _latestAnchorInfo;

        public InMemoryAnchorService()
        {
            _latestAnchorInfo = null;
        }

        public Task CreateAnchorAsync(AnchorInfo anchorInfo)
        {
            _latestAnchorInfo = anchorInfo;
            return Task.CompletedTask;
        }

        public Task<AnchorInfo?> TryGetLatestAnchorAsync()
        {
            return Task.FromResult(_latestAnchorInfo);
        }
    }
}