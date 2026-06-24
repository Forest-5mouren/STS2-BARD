using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers.Counters;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

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
        new DamageVar(3m, ValueProp.Move),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar(_calculatedHitsKey).WithMultiplier((card, _) =>
        {
            // 从隐藏计数器读取已打出的乐曲牌数量
            var power = card.Owner.Creature?.GetPower<SongsPlayedCounter>();
            return (int?)(power?.Amount) ?? 0;
        })
    };

        public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song];

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

        // 从隐藏计数器读取
        var power = Owner.Creature.GetPower<SongsPlayedCounter>();
        int hitCount = (int?)(power?.Amount) ?? 0;

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(hitCount)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitVfxNode(t => NStabVfx.Create(t, facingEnemies: true))
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
}




