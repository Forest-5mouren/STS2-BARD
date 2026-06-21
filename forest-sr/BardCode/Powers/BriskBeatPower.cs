using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 轻快节拍能力：每回合开始时抽1张牌，持续若干回合
/// </summary>
[RegisterPower]
public sealed class BriskBeatPower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// 修改手牌抽牌数
    /// </summary>
    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner?.Player) return count;
        return count + 1m;  // +1抽牌
    }

    /// <summary>
    /// 能量重置后（回合开始时）减少持续时间
    /// </summary>
    public override async Task AfterEnergyReset(Player player)
    {
        if (player == Owner?.Player)
        {
            await PowerCmd.ModifyAmount(new BlockingPlayerChoiceContext(), this, -1m, null, null, false);
        }

        if (Amount <= 0)
        {
            Flash();
            await PowerCmd.Remove(this);
        }
    }
}
