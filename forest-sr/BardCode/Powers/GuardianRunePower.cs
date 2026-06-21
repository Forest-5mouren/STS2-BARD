using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 守卫刻文能力：失去格挡时对全体敌人造成等量伤害
/// </summary>
[RegisterPower]
public sealed class GuardianRunePower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// 受到伤害后触发（格挡被消耗时）
    /// </summary>
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        // 只对自己生效
        if (target != Owner) return;

        // 如果实际消耗了格挡（格挡被用来抵挡伤害）
        if (result.BlockedDamage > 0)
        {
            // 对所有敌人造成等量伤害
            await CreatureCmd.Damage(
                choiceContext,
                Owner.CombatState.HittableEnemies,
                result.BlockedDamage,
                ValueProp.Unblockable | ValueProp.Unpowered,
                Owner,
                null
            );
        }
    }

    /// <summary>
    /// 回合开始时减少层数
    /// </summary>
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner?.Player) return;
        await PowerCmd.TickDownDuration(this);
    }
}
