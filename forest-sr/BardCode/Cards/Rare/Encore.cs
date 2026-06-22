using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 安可｜Encore
/// 效果：造成 {damage} 点伤害。本场战斗每打出过一张乐曲牌，就造成一次伤害。
/// 升级：基础伤害 6→8
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Encore : BardCard
{
    private const string _calculatedHitsKey = "CalculatedHits";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(6m, ValueProp.Move),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar(_calculatedHitsKey).WithMultiplier((card, _) =>
            CombatManager.Instance.History.CardPlaysStarted
                .Count(e => e.CardPlay.Card.Owner == card.Owner
                            && e.CardPlay.Card.HasModKeyword(BardKeywords.Song))  // ✅ 修正：使用 HasModKeyword
        )
    };

    public Encore() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    // 升级：伤害 6 → 8
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        int hitCount = (int)((CalculatedVar)DynamicVars[_calculatedHitsKey]).Calculate(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(hitCount)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitVfxNode(t => NStabVfx.Create(t, facingEnemies: true))
            .WithHitFx("vfx/vfx_attack_slash", null, "encore_sword.mp3")
            .Execute(choiceContext);
    }
}