using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 英雄气概｜Heroism
/// 效果：目标每回合开始时获得临时生命值（格挡）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Heroism : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyPlayer;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：持续时间 + 每回合格挡值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ModCardVars.Int("duration", 3),
        new BlockVar(5,ValueProp.Move)
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Magic];

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    public Heroism() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Creature target = cardPlay.Target ?? Owner.Creature;
        int duration = DynamicVars["duration"].IntValue;

        // 施加英雄气概效果
        var power = await PowerCmd.Apply<HeroismPower>(
            target,
            duration,
            Owner.Creature,
            this
        );

    }

    protected override void OnUpgrade()
    {
        // 升级：持续时间 +1（3 → 4）
        //DynamicVars["duration"].UpgradeValueBy(1);
        // 可选：格挡值 +2（5 → 7）
        DynamicVars.Block.UpgradeValueBy(2);
    }
}