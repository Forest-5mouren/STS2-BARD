using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 奥术回响能力：累计和声层数，达到阈值时从消耗堆回收法术，回合结束时清零
/// </summary>
public sealed class ArcaneEchoPower : BardPower
{
    private int _currentHarmony = 0;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    // 监听法术/乐曲打出
    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return;

        bool isMagic = cardPlay.Card.HasModKeyword(BardKeywords.Magic);
        bool isSong = cardPlay.Card.HasModKeyword(BardKeywords.Song);
        if (!isMagic && !isSong) return;

        _currentHarmony++;
        int threshold = (int)Amount;

        // 达到阈值就触发回收，不清零（回合结束才清零）
        if (_currentHarmony == threshold)
        {
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

    // 玩家回合开始时清零和声计数（替代 v1.0.7 移除的 AfterTurnEnd）
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner?.Player) return;
        _currentHarmony = 0;
        await Task.CompletedTask;
    }
}

