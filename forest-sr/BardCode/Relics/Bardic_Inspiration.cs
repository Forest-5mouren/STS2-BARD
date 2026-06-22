using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Relics;

/// <summary>
/// 诗人激励
/// 效果：每打出一张法术或乐曲牌，和声+1。
/// 回合开始时，全体队友获得和声层数的活力，然后和声清零。
/// </summary>
[RegisterRelic(typeof(BardRelicPool))]
public sealed class BardicInspiration : BardRelics
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    // 打出法术或乐曲牌时增加和声计数
    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner) return;
        if (cardPlay.Card.HasModKeyword(BardKeywords.Song) ||
            cardPlay.Card.HasModKeyword(BardKeywords.Magic))
        {
            HarmonyTracker.Count++;
            Flash();
        }
        await Task.CompletedTask;
    }

    // 回合开始时：全体队友获得对应层数的活力，和声清零
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner || player.Creature?.IsDead != false) return;

        if (HarmonyTracker.Count > 0)
        {
            foreach (Creature ally in player.Creature.CombatState.Allies)
            {
                await PowerCmd.Apply<VigorPower>(ctx, ally, HarmonyTracker.Count, player.Creature, null);
            }
            HarmonyTracker.Count = 0;
        }
    }
}


