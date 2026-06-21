using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Relics;

/// <summary>
/// 蛇笛｜Snake Charmer Flute
/// 效果：在第一回合开始时，将 {count} 张升级过的蛇咬牌加入手牌。
/// </summary>
[RegisterRelic(typeof(BardRelicPool))]
public sealed class SnakeCharmerFlute : BardRelics
{
    private const string _countKey = "count";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_countKey, 1)   // 添加的卡牌数量
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<PoisonPower>(),
        HoverTipFactory.FromCard<Snakebite>()      // 显示 Snakebite 卡牌预览
    ];

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        // 只对遗物拥有者生效，且只在第一回合
        if (player == Owner && player.Creature.CombatState.RoundNumber == 1)
        {
            int count = DynamicVars[_countKey].IntValue;
            List<CardModel> cardsToAdd = new List<CardModel>();

            for (int i = 0; i < count; i++)
            {
                // 创建蛇咬牌
                CardModel snakebite = Owner.Creature.CombatState.CreateCard<Snakebite>(Owner);

                // 升级卡牌
                CardCmd.Upgrade(snakebite);

                // ✅ 添加乐曲关键词（使用 AddModKeyword）
                snakebite.AddModKeyword(BardKeywords.Song);

                cardsToAdd.Add(snakebite);
            }

            // 将生成的卡牌添加到手牌
            await CardPileCmd.AddGeneratedCardsToCombat(cardsToAdd, PileType.Hand, Owner, CardPilePosition.Random);
        }
    }
}

