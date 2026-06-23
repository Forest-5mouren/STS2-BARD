using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 尖锐刺耳｜EarPiercingShriek
/// 效果：对所有敌人造成 {Damage} 点伤害，给予 {WeakPower} 层虚弱。吟唱。乐曲。
/// 升级：伤害 16→19，虚弱 1→2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class EarPiercingShriek : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(16, ValueProp.Move),
        new PowerVar<WeakPower>(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Chant, BardKeywords.Song];

    public EarPiercingShriek() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        // 对所有敌人造成伤害
        await CreatureCmd.Damage(
            ctx,
            CombatState!.HittableEnemies,
            DynamicVars.Damage.IntValue,
            ValueProp.Move,
            Owner.Creature,
            this);

        // 对所有敌人给予虚弱
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(ctx, enemy, DynamicVars.Weak.IntValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars.Weak.UpgradeValueBy(1);
    }
}
