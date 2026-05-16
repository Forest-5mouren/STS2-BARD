using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 韵律风暴能力：获得活力时造成AOE伤害，使用法术/乐曲牌时获得活力
/// </summary>
[RegisterPower]
public sealed class RhythmStormPower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    /// <summary>
    /// 设置数值
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 3,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),   // AOE伤害
        new PowerVar<VigorPower>( 2)     // 获得活力层数
    ];

    /// <summary>
    /// 卡牌打出后触发：使用法术牌或乐曲牌时获得活力
    /// </summary>
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        // 只对自己打出的牌生效
        if (cardPlay.Card.Owner.Creature != Owner) return;

        // ✅ 使用 HasModKeyword 检查
        bool isMagic = cardPlay.Card.HasModKeyword(BardKeywords.Magic);
        bool isSong = cardPlay.Card.HasModKeyword(BardKeywords.Song);

        if (!isMagic && !isSong) return;

        Flash();

        await PowerCmd.Apply<VigorPower>(
            Owner,
            DynamicVars["VigorPower"].IntValue * base.Amount,  //3*
            Owner,
            cardPlay.Card
        );
    }

    /// <summary>
    /// 当其他Power层数变化时触发（获得活力时）
    /// </summary>
    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        // 只关注活力 Power 的增加
        if (!(power is VigorPower)) return;
        if (amount <= 0) return;
        if (power.Owner != Owner) return;

        Flash();

        // 对所有敌人造成伤害
        await CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(),
            Owner.CombatState.HittableEnemies,
            DynamicVars.Damage.IntValue * base.Amount,
            ValueProp.Unpowered,
            Owner,
            null
        );
    }
}