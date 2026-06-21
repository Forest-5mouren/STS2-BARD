using Forest_Sr.Bard;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Cards.KeyWord;

/// <summary>
/// 吟游诗人模组的自定义关键词注册类
/// </summary>
[RegisterOwnedCardKeyword(nameof(Magic), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Song), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Chant), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
public class BardKeywords
{
    /// <summary>关键字ID（用于 RegisteredKeywordIds）</summary>
    public static readonly string MagicId = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(Magic));
    /// <summary>CardKeyword对象（用于 HasModKeyword / AddModKeyword）</summary>
    public static readonly CardKeyword Magic = MagicId.GetModCardKeyword();

    /// <summary>关键字ID（用于 RegisteredKeywordIds）</summary>
    public static readonly string SongId = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(Song));
    /// <summary>CardKeyword对象</summary>
    public static readonly CardKeyword Song = SongId.GetModCardKeyword();

    /// <summary>关键字ID（用于 RegisteredKeywordIds）</summary>
    public static readonly string ChantId = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(Chant));
    /// <summary>CardKeyword对象</summary>
    public static readonly CardKeyword Chant = ChantId.GetModCardKeyword();
}
