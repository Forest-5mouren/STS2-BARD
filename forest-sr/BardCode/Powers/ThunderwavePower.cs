using Forest_Sr.BardCode.Cards.Common;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Forest_Sr.BardCode.Powers;


public class ThunderwavePower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Thunderwave>();

    protected override bool IsPositive => false;
}