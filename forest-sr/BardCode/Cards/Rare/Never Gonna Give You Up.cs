using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// Never Gonna Give You Up
/// 效果：从消耗牌堆回收 {Cards} 张牌。
/// 升级：回收 1 → 2 张牌，费用 1 → 0
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class NeverGonnaGiveYouUp : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new CardsVar(1)
    };

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song, CardKeyword.Exhaust];

    public NeverGonnaGiveYouUp() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int recycleCount = DynamicVars.Cards.IntValue;
        CardPile exhaustPile = PileType.Exhaust.GetPile(Owner);

        if (!exhaustPile.Cards.Any()) return;

        int maxRecycle = System.Math.Min(
            recycleCount,
            exhaustPile.Cards.Count
        );

        maxRecycle = System.Math.Min(
            maxRecycle,
            10 - PileType.Hand.GetPile(Owner).Cards.Count
        );

        if (maxRecycle <= 0) return;

        var selectedCards = await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            exhaustPile.Cards,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, maxRecycle)
        );

        if (selectedCards.Any())
        {
            await CardPileCmd.Add(selectedCards, PileType.Hand);
        }
    }
}
