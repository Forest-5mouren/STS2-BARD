using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Basic;

[RegisterCard(typeof(BardCardPool))]
public sealed class ViciousMockery : BardCard
{
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    // 基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move),
        new PowerVar<WeakPower>(1)
    ];

    // 自定义关键词（需要先注册）
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic];

    public ViciousMockery() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 造成伤害
        await CreatureCmd.Damage(
            choiceContext,
            cardPlay.Target,
            DynamicVars.Damage.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered,
            Owner.Creature,
            this
        );

        // 施加虚弱（使用正确的 5 参数重载）
        await PowerCmd.Apply<WeakPower>(choiceContext, 
            cardPlay.Target,                  // 目标
            DynamicVars.Weak.BaseValue,       // 层数
            Owner.Creature,                   // 来源
            this);                            // 关联卡牌
    }

    protected override void OnUpgrade()
    {
        // 升级增加伤害
        DynamicVars.Damage.UpgradeValueBy(4);

        // 可选：升级后能量消耗变化（-1 表示减1费，但已经是0费所以可能不需要）
        // EnergyCost.UpgradeBy(-1);  // 需要确认基类是否有此属性
    }
}
