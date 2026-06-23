using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Relics;

[RegisterRelic(typeof(BardRelicPool))]
public sealed class BetterInspiration : BardRelics
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override bool ShowCounter => true;
    public override int DisplayAmount => (int)(Owner?.Creature?.GetPower<HarmonyPower>()?.Amount ?? 0);

    private async Task IncrementHarmony(PlayerChoiceContext ctx)
    {
        var creature = Owner?.Creature;
        if (creature == null) return;

        var power = creature.GetPower<HarmonyPower>();
        if (power == null)
        {
            power = (HarmonyPower)ModelDb.Power<HarmonyPower>().ToMutable();
            power.ApplyInternal(creature, 1, silent: true);
        }
        else
        {
            power.SetAmount(power.Amount + 1);
        }

        InvokeDisplayAmountChanged();
        Flash();
        await Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner) return;
        if (cardPlay.Card.HasModKeyword(BardKeywords.Song) ||
            cardPlay.Card.HasModKeyword(BardKeywords.Magic))
        {
            await IncrementHarmony(ctx);
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner || player.Creature?.IsDead != false) return;

        var power = player.Creature.GetPower<HarmonyPower>();

        if (power != null && power.Amount > 0)
        {
            int harmonyAmount = (int)power.Amount;

            foreach (Creature ally in player.Creature.CombatState.Allies)
            {
                await PowerCmd.Apply<VigorPower>(ctx, ally, harmonyAmount, player.Creature, null);
            }
        }

        // 即使和为0，也要调为2（保留基础和声）
        if (power == null)
        {
            power = (HarmonyPower)ModelDb.Power<HarmonyPower>().ToMutable();
            power.ApplyInternal(player.Creature, 2, silent: true);
        }
        else
        {
            power.SetAmount(2);
        }

        InvokeDisplayAmountChanged();
    }
}
