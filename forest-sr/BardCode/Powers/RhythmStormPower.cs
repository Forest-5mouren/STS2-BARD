using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
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
public sealed class RhythmStormPower : BardPower{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3, ValueProp.Move),
        new PowerVar<VigorPower>(2)
    ];

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return;

        bool isMagic = cardPlay.Card.HasModKeyword(BardKeywords.Magic);
        bool isSong = cardPlay.Card.HasModKeyword(BardKeywords.Song);
        if (!isMagic && !isSong) return;

        Flash();
        await PowerCmd.Apply<VigorPower>(context,
            Owner,
            DynamicVars["VigorPower"].IntValue * base.Amount,
            Owner,
            cardPlay.Card);
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!(power is VigorPower)) return;
        if (amount <= 0) return;
        if (power.Owner != Owner) return;

        Flash();
        await CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(),
            Owner.CombatState.HittableEnemies,
            DynamicVars.Damage.IntValue * base.Amount,
            ValueProp.Unpowered,
            Owner,
            null);
    }
}
