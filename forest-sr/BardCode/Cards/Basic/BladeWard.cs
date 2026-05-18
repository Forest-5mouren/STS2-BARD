using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Character;
using Forest_Sr.BardCode.Powers;
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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Basic;

[RegisterCard(typeof(BardCardPool))]
public sealed class BladeWard : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    private const string _powerVarName = "BladeWard";

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];
    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Magic];

    // 基础数值：格挡 + 自定义能力层数
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(1, ValueProp.Move),
        new DynamicVar(_powerVarName, 1)
    ];

    public BladeWard() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 1. 播放施法动画
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        // 2. 获得格挡
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue, ValueProp.Move, cardPlay);

        // 3. 施加剑刃防护能力（使用正确的 5 参数重载）
        await PowerCmd.Apply<BladeWardPower>(
            Owner.Creature,                          // 目标
            DynamicVars[_powerVarName].BaseValue,    // 层数
            Owner.Creature,                          // 来源
            this);                                   // 关联卡牌
    }

    protected override void OnUpgrade()
    {
        // 升级时增加数值
        DynamicVars.Block.UpgradeValueBy(2);
        AddKeyword(CardKeyword.Retain);
    }
}