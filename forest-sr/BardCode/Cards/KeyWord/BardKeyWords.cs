using Forest_Sr.Bard;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Cards.KeyWord;

/// <summary>
/// 吟游诗人模组的自定义关键词注册类
/// </summary>
[RegisterOwnedCardKeyword(nameof(Magic), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]

[RegisterOwnedCardKeyword(nameof(Song), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
public class BardKeywords
{
    public static readonly string Magic = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(Magic));

    public static readonly string Song = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(Song));
}