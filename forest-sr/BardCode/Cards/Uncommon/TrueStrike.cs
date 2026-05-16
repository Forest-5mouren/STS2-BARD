using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 克敌机先｜TrueStrike
/// 效果：移除敌人的人工制品，然后施加 {vulnerable} 层易伤。
/// 升级：易伤 2 → 3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class TrueStrike : BardCard
{
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(2)   // 易伤层数
    ];

    // 关键词：消耗、魔法
    protected override IEnumerable<string> RegisteredKeywordIds => [
        "EXHAUST",
        BardKeywords.Magic
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    public TrueStrike() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    // 升级：易伤 2 → 3
    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 如果目标有人工制品，移除它
        if (cardPlay.Target.HasPower<ArtifactPower>())
        {
            await PowerCmd.Remove<ArtifactPower>(cardPlay.Target);
        }

        // 施加易伤
        int vulnerableAmount = DynamicVars.Vulnerable.IntValue;
        await PowerCmd.Apply<VulnerablePower>(
            cardPlay.Target,
            DynamicVars.Vulnerable.BaseValue,
            Owner.Creature,
            this
        );
    }
}