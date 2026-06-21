using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Forest_Sr.BardCode.Powers;
/// <summary>
/// 即兴能力：每回合开始时随机获得一首乐曲（排除吟唱牌）
/// </summary>
[RegisterPower]
public sealed class ImprovisePower : BardPower{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner.Player) return;

        var allUnlockedCards = player.Character.CardPool
            .GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
            .ToList();

        var songCards = allUnlockedCards
            .Where(card => card.HasModKeyword(BardKeywords.Song) && !card.HasModKeyword(BardKeywords.Chant))
            .ToList();

        if (songCards.Count == 0) return;

        int amount = Amount;
        CardModel[] cardsToAdd = new CardModel[amount];
        Rng combatCardGeneration = player.RunState.Rng.CombatCardGeneration;
        for (int i = 0; i < amount; i++)
        {
            var randomCard = CardFactory.GetDistinctForCombat(player, songCards, 1, combatCardGeneration).First();
            cardsToAdd[i] = randomCard;
            CardCmd.ApplyKeyword(randomCard, CardKeyword.Exhaust);
        }

        Flash();
        await CardPileCmd.AddGeneratedCardsToCombat(cardsToAdd, PileType.Hand, Owner.Player, CardPilePosition.Random);
    }
}
