using System;
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
}

public interface IAnchorService
{
    Task CreateAnchor(AnchorInfo anchorInfo);

    Task<AnchorInfo?> TryGetLatestAnchor();
}