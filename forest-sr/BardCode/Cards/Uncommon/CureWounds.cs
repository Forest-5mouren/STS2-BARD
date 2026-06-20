using Forest_Sr.BardCode.Cards.KeyWord;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 治疗术｜CureWounds
/// 效果：回复 {heal} 点生命。
/// 升级：治疗量 8 → 11
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class CureWounds : BardCard
{
    private const string _healKey = "heal";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(8)
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Magic];
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];



    public CureWounds() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer)
    {
    }

    // 升级：治疗量 8 → 11
    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 确定治疗目标（如果没有指定目标，则治疗自己）
        Creature target = cardPlay.Target ?? Owner.Creature;

        // 播放治疗特效
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color("#FFD1DC"));

        // 回复生命
        int healAmount = DynamicVars.Heal.IntValue;
        await CreatureCmd.Heal(target, healAmount);
    }
}