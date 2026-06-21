using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 把酒迎欢｜Bottoms Up
/// 效果：获得 {strength} 层力量，{vigor} 层活力，往弃牌堆加入 {dizzyCount} 张眩晕。
/// 升级：眩晕数量 2 → 1
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class BottomsUp : BardCard
{
    private const string _strengthKey = "strength";
    private const string _vigorKey = "vigor";
    private const string _dizzyCountKey = "dizzyCount";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<StrengthPower>( 1),     // 力量层数
        new PowerVar<VigorPower>( 4),        // 活力层数
        new DynamicVar(_dizzyCountKey, 2)    // 眩晕加入数量
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<VigorPower>(),
        HoverTipFactory.FromCard<Dazed>()
    ];

    public BottomsUp() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：眩晕数量 2 → 1
    protected override void OnUpgrade()
    {
        DynamicVars[_dizzyCountKey].UpgradeValueBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 播放饮酒特效（红色/酒色）
        NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, new Color("#E74C3C"));

        // 播放施法动画
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        // 获得力量
        await PowerCmd.Apply<StrengthPower>(choiceContext, 
            Owner.Creature,
            DynamicVars["StrengthPower"].IntValue,
            Owner.Creature,
            this
        );

        // 获得活力
        await PowerCmd.Apply<VigorPower>(choiceContext, 
            Owner.Creature,
            DynamicVars["VigorPower"].IntValue,
            Owner.Creature,
            this
        );

        // 往弃牌堆加入眩晕
        int dizzyCount = DynamicVars[_dizzyCountKey].IntValue;
        for (int i = 0; i < dizzyCount; i++)
        {
            CardModel dizzy = CombatState.CreateCard<Dazed>(Owner);
            await CardPileCmd.AddGeneratedCardToCombat(dizzy, PileType.Discard, Owner, CardPilePosition.Random);
        }
    }
}

