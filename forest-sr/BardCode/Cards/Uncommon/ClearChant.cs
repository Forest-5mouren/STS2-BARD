using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 清晰赞歌｜ClearChant
/// 效果：吟唱。下回合开始时，从抽牌堆搜索 {draw} 张法术牌加入手牌。获得 1 层易伤。乐曲。
/// 升级：搜索 2→3 张
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ClearChant : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(2),
        new PowerVar<VulnerablePower>(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song, BardKeywords.Chant];

    public ClearChant() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var chant = await PowerCmd.Apply<ClearChantChant>(ctx, Owner.Creature, 1, Owner.Creature, this);
        chant.DrawAmount = DynamicVars.Cards.IntValue;
        chant.VulnerableAmount = DynamicVars["VulnerablePower"].IntValue;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}

