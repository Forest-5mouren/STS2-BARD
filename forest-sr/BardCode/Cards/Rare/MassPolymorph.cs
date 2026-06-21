using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Forest_Sr.BardCode.Cards.Rare;
/// <summary>
/// 群体变形术｜MassPolymorph
/// 效果：消耗。选择手牌中任意张牌，将其变为随机法术牌。
/// 升级：变形得到的卡牌自动升级
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class MassPolymorph : BardCard{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic, CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Transform)
    ];

    public MassPolymorph() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        CardSelectorPrefs selectPrefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 0, 999);
        List<CardModel> selectedCards = (await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            selectPrefs,
            null,
            this)).ToList();

        if (selectedCards.Count == 0) return;

        foreach (CardModel card in selectedCards)
        {
            CardPileAddResult result = await CardCmd.TransformToRandom(
                card,
                Owner.RunState.Rng.CombatCardGeneration
            );
            CardModel transformedCard = result.cardAdded;
            if (transformedCard != null && IsUpgraded)
            {
                CardCmd.Upgrade(transformedCard);
            }
        }
    }

    protected override void OnUpgrade() { }
}
