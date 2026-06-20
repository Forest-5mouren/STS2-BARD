using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Cards.Other;
using Forest_Sr.BardCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MegaCrit.Sts2.Core.Models.Monsters.KnowledgeDemon;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 强化属性｜Enhance Ability
/// 效果：选择并获得以下一种动物之力：
/// 公牛之力：获得 {strength} 层力量
/// 猫之优雅：获得 {dexterity} 层敏捷
/// 狐之狡黠：抽 {DynamicVars.Cards.IntValue} 张牌，本回合下一张法术牌费用-1
/// 升级：力量 2→3，敏捷 2→3，抽牌 1→2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class EnhanceAbility : BardCard
{
    
    
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<StrengthPower>(2),     // 力量层数
        new PowerVar<DexterityPower>(2),    // 敏捷层数
        new CardsVar(1)                     // 抽牌数量
    ];

    // 关键词：魔法
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Magic
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>(),
        HoverTipFactory.Static(StaticHoverTip.CardReward)
    ];

    public EnhanceAbility() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：力量 2→3，敏捷 2→3，抽牌 1→2
    protected override void OnUpgrade()
    {
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
        DynamicVars["DexterityPower"].UpgradeValueBy(1);
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 播放施法特效
        NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, new Color("#9B59B6"));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);


        // 创建选择卡牌
        var bullStrength = CombatState.CreateCard<BullStrength>(Owner);

        var catGrace = CombatState.CreateCard<CatGrace>(Owner);

        var foxCunning = CombatState.CreateCard<FoxCunning>(Owner);

        var optionCards = new List<CardModel> { bullStrength, catGrace, foxCunning };

        // 弹出选择界面
        CardModel selected = await CardSelectCmd.FromChooseACardScreen(
            choiceContext,
            optionCards,
            Owner,
            canSkip: true
        );

        // 处理被选中的卡牌效果
        if (selected != null)
        {
            if (selected.Id == ModelDb.Card<BullStrength>().Id)
            {
                await PowerCmd.Apply<StrengthPower>(
                    Owner.Creature,
                    DynamicVars["StrengthPower"].IntValue,
                    Owner.Creature,
                    this);
            }
            else if (selected.Id == ModelDb.Card<CatGrace>().Id)
            {
                await PowerCmd.Apply<DexterityPower>(
                    Owner.Creature,
                    DynamicVars["DexterityPower"].IntValue,
                    Owner.Creature,
                    this);
            }
            else if (selected.Id == ModelDb.Card<FoxCunning>().Id)
            {
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
                await PowerCmd.Apply<NextSpellCostReductionPower>(
                    Owner.Creature,
                    1m,
                    Owner.Creature,
                    this);
            }
        }
    }
}

