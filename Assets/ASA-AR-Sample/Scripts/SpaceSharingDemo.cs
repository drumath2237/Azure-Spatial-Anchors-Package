using System.Threading.Tasks;
using Microsoft.Azure.SpatialAnchors.Unity;
using TMPro;
using UnityEngine;

internal enum AnchorOperationStatus
{
    UnderstandingEnvironment,
    ReadyForCreateAnchor,
    CreatingAnchor,
    AnchorCreated,
    FindingAnchor,
    AnchorFound,
    None
}

public class SpaceSharingDemo : MonoBehaviour
{
    [SerializeField] private SpatialAnchorManager spatialAnchorManager;

    [SerializeField] private CloudNativeAnchor creationAnchor;
    [SerializeField] private CloudNativeAnchor finderAnchor;

    private IAnchorService _anchorService;

    private AnchorCreator _anchorCreator;
    private AnchorFinder _anchorFinder;

    [SerializeField] private TextMeshProUGUI sessionReadyText;

    private AnchorOperationStatus _anchorOperationStatus;


    private void Start()
    {
        _anchorService = new InMemoryAnchorService();
        _anchorCreator = new AnchorCreator(spatialAnchorManager, _anchorService);
        _anchorFinder = new AnchorFinder(spatialAnchorManager, _anchorService);
        _anchorOperationStatus = AnchorOperationStatus.None;
    }

    public async void UpdateStatus()
    {
        _anchorOperationStatus = _anchorOperationStatus switch
        {
            AnchorOperationStatus.None or AnchorOperationStatus.AnchorFound
                => await StartCreation(_anchorOperationStatus),
            AnchorOperationStatus.UnderstandingEnvironment
                => await CreateAnchorAsync(_anchorOperationStatus),
        };
    }

    private async Task<AnchorOperationStatus> StartCreation(AnchorOperationStatus status)
    {
        if (status is not AnchorOperationStatus.None or AnchorOperationStatus.AnchorFound)
        {
            return status;
        }

        await _anchorCreator.StartSessionAsync();
        return AnchorOperationStatus.UnderstandingEnvironment;
    }

    private async Task<AnchorOperationStatus> CreateAnchorAsync(AnchorOperationStatus status)
    {
        if (status != AnchorOperationStatus.UnderstandingEnvironment || !spatialAnchorManager.IsReadyForCreate)
        {
            return status;
        }

        await _anchorCreator.CreateAnchorAsync(creationAnchor);
        _anchorCreator.DestroySession();
        return AnchorOperationStatus.AnchorCreated;
    }

    async Task<AnchorOperationStatus> StartFindingAnchor(AnchorOperationStatus status)
    {
        if (status != AnchorOperationStatus.AnchorCreated)
        {
            return status;
        }

        _anchorFinder.OnAnchorFound += OnAnchorFound;
        await _anchorFinder.StartSessionAsync();
        await _anchorFinder.StartFindAnchorAsync(finderAnchor);
        return AnchorOperationStatus.FindingAnchor;
    }


    private void Update()
    {
        sessionReadyText.text = _anchorCreator.IsReadyForCreateAnchor ? "ready for create" : "not ready";
    }


    private void OnAnchorFound(CloudNativeAnchor nativeAnchor)
    {
        _anchorFinder.DestroySession();
        _anchorFinder.OnAnchorFound -= OnAnchorFound;
    }
}