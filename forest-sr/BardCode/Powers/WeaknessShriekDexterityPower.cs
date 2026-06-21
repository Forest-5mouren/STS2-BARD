using MegaCrit.Sts2.Core.Models;
using Forest_Sr.BardCode.Cards.Uncommon;

namespace MegaCrit.Sts2.Core.Models.Powers;

/// <summary>
/// 虚弱尖叫 - 临时敏捷减益
/// </summary>
public class WeaknessShriekDexterityPower : TemporaryDexterityPower
{
    public override AbstractModel OriginModel => ModelDb.Card<WeaknessShriek>();
}
