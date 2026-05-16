using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 超频施法｜OverclockCasting
/// 效果：能力。你的所有法术牌获得消耗和再次打出。
/// 升级：移除虚无
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class OverclockCasting : BardCard
{
    private const string _markerKey = "marker";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_markerKey, 1)   // 能力标记
    ];

    // 关键词：虚无（升级后移除）
    protected override IEnumerable<string> RegisteredKeywordIds => ["ETHEREAL"];

    public OverclockCasting() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：移除虚无（已通过 RegisteredKeywordIds 处理）
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Eternal);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        int markerAmount = DynamicVars[_markerKey].IntValue;

        // 施加超频施法能力Power
        await PowerCmd.Apply<OverclockCastingPower>(
            Owner.Creature,
            markerAmount,
            Owner.Creature,
            this
        );
    }
}