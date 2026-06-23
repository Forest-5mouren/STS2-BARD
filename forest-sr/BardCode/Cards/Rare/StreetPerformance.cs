using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 街头卖艺｜StreetPerformance
/// 效果：如果本回合打出过乐曲牌，获得 {bonusGold} 金币。消耗。
/// 升级：额外金币 15 → 20
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class StreetPerformance : BardCard
{

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new GoldVar(20)   // 额外金币
    ];

    // 关键词：消耗
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public StreetPerformance() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：额外金币 15 → 20
    protected override void OnUpgrade()
    {
        DynamicVars.Gold.UpgradeValueBy(5);
    }

    // 高亮条件：本回合打出过乐曲牌
    protected override bool ShouldGlowGoldInternal => HasPlayedSongThisTurn();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int bonusGold = DynamicVars.Gold.IntValue;

        // 检查本回合是否打出过乐曲牌
        if (HasPlayedSongThisTurn())
        {
            await PlayerCmd.GainGold(bonusGold, Owner);
        }
    }

    /// <summary>
    /// 检查本回合是否打出过乐曲牌
    /// </summary>
    private bool HasPlayedSongThisTurn()
    {
        if (CombatState == null) return false;

        return CombatManager.Instance.History.CardPlaysStarted
            .Any(e => e.CardPlay.Card.Owner == Owner
                && e.HappenedThisTurn(CombatState)
                && e.CardPlay.Card.HasModKeyword(BardKeywords.Song));
    }
}