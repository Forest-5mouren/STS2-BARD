using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 洪雷｜ThunderclapPower
/// 效果：本回合获得Debuff时，每有一层洪雷造成1点伤害
/// </summary>
[RegisterPower]
public sealed class ThunderclapPower : BardPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        // 官方写法：amount != 0 && 是Debuff && 目标是敌人  && 不是临时Power
        if (amount != 0m &&
            power.GetTypeForAmount(amount) == PowerType.Debuff &&
            power.Owner.IsEnemy &&
            !(power is ITemporaryPower))
        {
            Flash();

            int damage = Amount;
            await CreatureCmd.Damage(
                new ThrowingPlayerChoiceContext(),
                power.Owner,
                damage,
                ValueProp.Unpowered,
                Owner,
                null
            );
        }
    }

    // 回合结束时移除
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        await PowerCmd.Remove(this);
    }
}

