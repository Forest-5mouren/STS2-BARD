using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 旋律护身能力
/// 效果：每当你打出法术牌时，获得格挡
/// </summary>
[RegisterPower]
public sealed class MelodicWardPower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    /// <summary>
    /// 当打出卡牌时触发
    /// </summary>
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        // 检查是否是法术牌（使用 HasModKeyword）
        if (!cardPlay.Card.HasModKeyword(BardKeywords.Magic)) return;

        if (Owner == null) return;

        // 获得格挡
        await CreatureCmd.GainBlock(Owner, new BlockVar(base.Amount,ValueProp.Unpowered), null);
    }
}