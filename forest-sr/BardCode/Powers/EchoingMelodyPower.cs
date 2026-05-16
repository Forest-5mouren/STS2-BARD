using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

[RegisterPower]
public sealed class EchoingMelodyPower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;

        var creature = Owner;
        if (creature.CombatState.RoundNumber == 1) return;

        int lastRound = creature.CombatState.RoundNumber - 1;

        // ✅ 修正：使用 HasModKeyword 检查乐曲牌
        var entry = CombatManager.Instance.History.CardPlaysFinished
            .LastOrDefault(e => e.CardPlay.Card.Owner == player
                && e.RoundNumber == lastRound
                && e.CardPlay.Card.HasModKeyword(BardKeywords.Song)
                && !e.CardPlay.Card.IsDupe);

        if (entry != null)
        {
            Flash();
            CardModel dupe = entry.CardPlay.Card.CreateDupe();

            // 给复制牌添加消耗关键词
            dupe.AddKeyword(CardKeyword.Exhaust);

            await CardCmd.AutoPlay(choiceContext, dupe, null);
        }
    }
}