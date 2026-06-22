using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
namespace Forest_Sr.BardCode.Cards.Rare;
/// <summary>
/// 绝不认输｜NeverGiveUp
/// 效果：乐曲。能力。每当你即将死亡时，改为回复到 {healAmount} 点生命，消耗此能力并抽牌。
/// 升级：费用 3 → 2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class NeverGiveUp : BardCard
{
    const string _healAmountKey = "healAmount";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_healAmountKey, 5)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song, CardKeyword.Retain];

    public NeverGiveUp() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self) { }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<NeverGiveUpPower>(choiceContext,
            Owner.Creature,
            1,
            Owner.Creature,
            this);
    }
}
