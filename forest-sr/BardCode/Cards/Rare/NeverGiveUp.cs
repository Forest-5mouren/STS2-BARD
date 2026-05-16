using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 绝不认输｜NeverGiveUp
/// 效果：乐曲。能力。每当你即将死亡时，改为回复到 {healAmount} 点生命，消耗此能力并抽 {drawAmount} 张牌。
/// 升级：费用 3 → 2，回复 1 → 2，抽牌 3 → 4
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class NeverGiveUp : BardCard
{
    private const string _healAmountKey = "healAmount";
    private const string _drawAmountKey = "drawAmount";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_healAmountKey, 5),   // 回复生命值
        new DynamicVar(_drawAmountKey, 3)    // 抽牌数量
    ];

    // 关键词：乐曲、虚无（升级后移除虚无？注释没写，先保留）
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Song,
        "ETHEREAL"
    ];

    public NeverGiveUp() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：费用 3 → 2，回复 1 → 2，抽牌 3 → 4
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);                    // 3 → 2
        DynamicVars[_healAmountKey].UpgradeValueBy(1);  // 1 → 2
        DynamicVars[_drawAmountKey].UpgradeValueBy(1);  // 3 → 4
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        // 施加能力Power（层数为1，作为标记）
        await PowerCmd.Apply<NeverGiveUpPower>(
            Owner.Creature,
            1,
            Owner.Creature,
            this
        );
    }
}