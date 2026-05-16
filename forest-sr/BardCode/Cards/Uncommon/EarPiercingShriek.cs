using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 尖锐刺耳｜EarPiercingShriek
/// 效果：对所有敌人造成 {damage} 点伤害，给予 {weak} 层虚弱。
/// 升级：伤害 13→15，虚弱 1→2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class EarPiercingShriek : BardCard
{
    
    private const string _weakKey = "weak";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 13,ValueProp.Move),    // 伤害值
        new PowerVar<WeakPower>( 1)        // 虚弱层数
    ];

    // 关键词：乐曲
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Song
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    public EarPiercingShriek() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    // 升级：伤害 12→14，虚弱 2→3
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);  // 12 → 14
        DynamicVars.Weak.UpgradeValueBy(1);    // 2 → 3
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 造成伤害
        
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);

        // 给予虚弱
        
        await PowerCmd.Apply<WeakPower>(
            CombatState.HittableEnemies,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this
        );
    }
}