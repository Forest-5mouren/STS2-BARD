using Forest_Sr.BardCode.Cards.KeyWord;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 大步奔行｜Longstrider
/// 效果：使一个友方获得 {dexterity} 层敏捷，并抽 {draw} 张牌。
/// 升级：抽牌 1 → 2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class LongStrider : BardCard
{
    
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<DexterityPower>( 1),   // 1层敏捷
        new CardsVar( 1)         // 抽1张牌
    ];

    // 关键词：魔法
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        BardKeywords.Magic
    ];

    public LongStrider() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer)
    {
    }

    // 升级：抽牌 1 → 2
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        // 敏捷不变（保持1层）
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 统一处理目标
        Creature target = cardPlay.Target ?? Owner.Creature;
        Player targetPlayer = target.Player ?? Owner;

        // 施加敏捷
        int dexterityAmount = DynamicVars.Dexterity.IntValue;
        await PowerCmd.Apply<DexterityPower>(choiceContext, 
            target,
            dexterityAmount,
            Owner.Creature,
            this
        );

        // 抽牌
        int drawAmount = DynamicVars.Cards.IntValue;
        await CardPileCmd.Draw(choiceContext, drawAmount, targetPlayer);
    }
}
