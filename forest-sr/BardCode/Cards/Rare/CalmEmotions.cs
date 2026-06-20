using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 安定心神｜CalmEmotions
/// 效果：清除所有负面效果（Debuff）。
/// 升级：费用 3 → 2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class CalmEmotions : BardCard
{
    // 无动态变量
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    // 关键词：消耗、魔法
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Magic
    ];
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public CalmEmotions() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyPlayer)
    {
    }

    // 升级：费用 3 → 2
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 统一处理目标
        Creature target = cardPlay.Target ?? Owner.Creature;

        // 清除目标所有负面效果
        await ClearDebuffs(target);
    }

    /// <summary>
    /// 清除生物身上的所有负面效果
    /// </summary>
    private async Task ClearDebuffs(Creature target)
    {
        // 获取目标身上的所有Power
        var powers = target.Powers.ToList();

        // 清除所有负面效果（Debuff）
        foreach (var power in powers)
        {
            if (power.Type == PowerType.Debuff)
            {
                await PowerCmd.Remove(power);
            }
        }
    }
}