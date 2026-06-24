using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 奥术回响能力：读取共享 HarmonyPower 追踪和声，达到阈值时从消耗堆回收法术
/// </summary>
public sealed class ArcaneEchoPower : BardPower
{
    // 防止本回合重复触发
    private bool _alreadyTriggeredThisTurn;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    // 监听法术/乐曲打出
    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return;

        bool isMagic = cardPlay.Card.HasModKeyword(BardKeywords.Magic);
        bool isSong = cardPlay.Card.HasModKeyword(BardKeywords.Song);
        if (!isMagic && !isSong) return;

        // 使用共享的 HarmonyPower（由遗物 BardicInspiration/BetterInspiration 维护）
        var harmonyPower = Owner!.GetPower<HarmonyPower>();
        int harmony = (int)(harmonyPower?.Amount ?? 0);
        int threshold = (int)Amount;

        // 达到阈值且本回合未触发过
        if (harmony >= threshold && !_alreadyTriggeredThisTurn)
        {
            _alreadyTriggeredThisTurn = true;
            Flash();

            var exhaustPile = PileType.Exhaust.GetPile(Owner.Player);
            var spells = exhaustPile?.Cards
                .Where(c => c.HasModKeyword(BardKeywords.Magic))
                .ToList();

            if (spells != null && spells.Count > 0)
            {
                var rng = Owner.Player.RunState.Rng.CombatCardGeneration;
                var selected = rng.NextItem(spells);
                await CardPileCmd.AddGeneratedCardToCombat(selected, PileType.Hand, Owner.Player, CardPilePosition.Random);
            }
        }
    }

    // 重置本回合的触发标记，HarmonyPower 由遗物负责管理
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner?.Player) return;
        _alreadyTriggeredThisTurn = false;
        await Task.CompletedTask;
    }
}
