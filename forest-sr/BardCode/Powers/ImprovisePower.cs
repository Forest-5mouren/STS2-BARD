using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
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
public sealed class ImprovisePower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    /// <summary>
    /// 抽牌前触发（每回合开始时）
    /// </summary>
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext)
    {
        // 只对自己生效
        if (player != Owner.Player) return;

        // 获取所有已解锁的乐曲牌
        var allUnlockedCards = player.Character.CardPool
            .GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
            .ToList();

        // 使用 HasModKeyword 检查乐曲牌，排除吟唱牌
        var songCards = allUnlockedCards
            .Where(card => card.HasModKeyword(BardKeywords.Song) && !card.HasModKeyword(BardKeywords.Chant))
            .ToList();

        if (songCards.Count == 0) return;

        // 随机选择 Amount 张乐曲牌（Amount 默认为1）
        int amount = Amount;
        CardModel[] cardsToAdd = new CardModel[amount];
        Rng combatCardGeneration = player.RunState.Rng.CombatCardGeneration;

        for (int i = 0; i < amount; i++)
        {
            var randomCard = CardFactory.GetDistinctForCombat(player, songCards, 1, combatCardGeneration).First();
            cardsToAdd[i] = randomCard;

            // 添加消耗关键词（参考官方使用 CardCmd.ApplyKeyword）
            CardCmd.ApplyKeyword(randomCard, CardKeyword.Exhaust);
        }

        // 闪烁提示
        Flash();

        // 将随机乐曲牌加入手牌
        await CardPileCmd.AddGeneratedCardsToCombat(cardsToAdd, PileType.Hand, addedByPlayer: true);
    }
}
