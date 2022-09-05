using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class AzureServerlessAnchorService : IAnchorService
{
    readonly string baseUrl;

    public AzureServerlessAnchorService(
        string apiBaseUrl = @"https://shoten13-anchorinfo.azurewebsites.net")
    {
        baseUrl = apiBaseUrl;
    }

    public async Task CreateAnchorAsync(AnchorInfo anchorInfo)
    {
        var requestUrl = baseUrl + $"/create?anchorKey={anchorInfo.anchorKey}&expireOn={anchorInfo.expireOn}";
        using var httpClient = new HttpClient();
        await httpClient.PostAsync(requestUrl, null);
    }

    public async Task<AnchorInfo?> TryGetLatestAnchorAsync()
    {
         var requestUrl = baseUrl + "/latest";
        using var httpClient = new HttpClient();
        using var response = await httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var textContent = await response.Content.ReadAsStringAsync();
        var responseAnchorInfo = JsonUtility.FromJson<AnchorInfo>(textContent);

        return responseAnchorInfo;
    }
}