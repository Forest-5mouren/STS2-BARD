using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 轰雷剑｜ThunderclapBlade
/// 效果：造成 {damage} 点伤害。使敌人获得 {thunderclap} 层洪雷。
/// 升级：伤害 6→8，洪雷层数 5→7
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ThunderclapBlade : BardCard
{
    
    private const string _thunderclapKey = "thunderclap";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 6, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),       // 伤害值
        new DynamicVar(_thunderclapKey, 5)   // 洪雷层数
    ];

    // 关键词：魔法
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Magic
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<ThunderclapPower>()
    ];

    public ThunderclapBlade() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    // 升级：伤害 6→8，洪雷层数 5→7
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);       // 6 → 8
        DynamicVars[_thunderclapKey].UpgradeValueBy(2);  // 5 → 7
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 1. 造成伤害
        
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        // 2. 给予洪雷效果
        int thunderclapAmount = DynamicVars[_thunderclapKey].IntValue;
        await PowerCmd.Apply<ThunderclapPower>(
            cardPlay.Target,
            thunderclapAmount,
            Owner.Creature,
            this
        );
    }
}