using System;
using System.Globalization;
using System.Threading.Tasks;

[Serializable]
public struct AnchorInfo
{
    /// <summary>
    /// 空間アンカーの識別子
    /// </summary>
    public string anchorKey;

    /// <summary>
    /// 破棄される時間を文字列化にシリアライズしたもの
    /// dateTime.toString()で生成される想定
    /// </summary>
    public string expireOn;

    public AnchorInfo(string key, DateTime expirationDate)
    {
        anchorKey = key;
        expireOn = expirationDate.ToString("yyyyMMddHHmmss");
    }
}

public interface IAnchorService
{
    Task CreateAnchorAsync(AnchorInfo anchorInfo);

    Task<AnchorInfo?> TryGetLatestAnchorAsync();
}