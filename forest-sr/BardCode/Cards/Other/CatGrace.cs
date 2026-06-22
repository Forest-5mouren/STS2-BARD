using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Forest_Sr.BardCode.Cards.Other
{
    public sealed class CatGrace : BardCard
    {
        public override bool CanBeGeneratedInCombat => false;

        public override int MaxUpgradeLevel => 0;

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new PowerVar<DexterityPower>(3m)
        };

        public CatGrace()
            : base(-1, CardType.Status, CardRarity.Status, TargetType.None)
        {
        }

        public async Task OnChosen(PlayerChoiceContext choiceContext)
        {
            await PowerCmd.Apply<DexterityPower>(choiceContext,
                base.Owner.Creature,
                base.DynamicVars["DexterityPower"].IntValue,
                base.Owner.Creature,
                this
            );
        }
    }
}
