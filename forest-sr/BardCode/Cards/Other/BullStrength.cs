using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Forest_Sr.BardCode.Cards.Other
{
    public sealed class BullStrength : BardCard
    {
        public override bool CanBeGeneratedInCombat => false;

        public override int MaxUpgradeLevel => 0;

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
             new PowerVar<StrengthPower>(3m)
        };

        public BullStrength()
            : base(-1, CardType.Status, CardRarity.Status, TargetType.None)
        {
        }

        public async Task OnChosen(PlayerChoiceContext choiceContext)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext,
                base.Owner.Creature,
                base.DynamicVars["StrengthPower"].IntValue,
                base.Owner.Creature,
                this
            );
        }
    }
}

