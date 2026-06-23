using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 英勇冲刺｜ValiantDash
/// 效果：消耗所有的活力，每消耗 {threshold} 层抽1张牌。
/// 升级：移除消耗，获得保留。
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ValiantDash : BardCard
{
    private const string _thresholdKey = "threshold";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_thresholdKey, 2)   // 每2层抽1张牌
    ];

    // 关键词：升级后无消耗，获得保留
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    public ValiantDash() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取当前的活力层数
        var vigorPower = Owner.Creature.GetPower<VigorPower>();
        int vigor = vigorPower?.Amount ?? 0;

        if (vigor > 0)
        {
            // 计算抽牌数量：每2层抽1张（向下取整）
            int card = (vigor - vigor % 2) / 2;
            // 抽牌
            await CardPileCmd.Draw(choiceContext, card, Owner);
            // 消耗所有活力
            await PowerCmd.Remove(vigorPower);
        }
    }
}