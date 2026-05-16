using Forest_Sr.BardCode.Cards.KeyWord;
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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Common;
//灾祸术
[RegisterCard(typeof(BardCardPool))]
public sealed class Bane : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<WeakPower>(3)
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Magic, "Exhaust"];

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    public Bane() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 施法动画
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);


        // 对每个可攻击的敌人施加虚弱
        foreach (Creature enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(
                enemy,                  // 目标
                DynamicVars.Weak.BaseValue,                 // 层数
                Owner.Creature,         // 来源
                this);                  // 关联卡牌
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Weak.UpgradeValueBy(2);  // 3 → 5 层虚弱
    }
}