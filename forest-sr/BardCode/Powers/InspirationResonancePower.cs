using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 灵感共鸣能力：打出乐曲牌时回复能量
/// </summary>
[RegisterPower]
public sealed class InspirationResonancePower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// 卡牌打出后触发
    /// </summary>
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        // 只对自己打出的牌生效
        if (cardPlay.Card.Owner.Creature != Owner) return;

        // ✅ 修正：使用 HasModKeyword 检查自定义关键词
        if (!cardPlay.Card.HasModKeyword(BardKeywords.Song)) return;

        // 回复能量
        int energyToGain = Amount;  // Amount 已经是 int
        if (energyToGain > 0)
        {
            Flash();
            await PlayerCmd.GainEnergy(energyToGain, Owner.Player);
        }
    }

    /// <summary>
    /// 回合结束时移除自身
    /// </summary>
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side)
        {
            await PowerCmd.Remove(this);
        }
    }
}