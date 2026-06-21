using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Relics;

/// <summary>
/// 大师的小提琴｜Virtuosa Violin
/// 效果：每当你打出乐曲牌时，对所有敌人施加 {doom} 层末日。
/// </summary>
[RegisterRelic(typeof(BardRelicPool))]
public sealed class VirtuosaViolin : BardRelics
{
    private const string _doomKey = "doom";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_doomKey, 3)   // 末日层数
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<DoomPower>()
    ];

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        // 检查是否是乐曲牌且是遗物拥有者打出的
        if (cardPlay.Card.HasModKeyword(BardKeywords.Song) && cardPlay.Card.Owner == Owner)
        {
            int doomAmount = DynamicVars[_doomKey].IntValue;

            // 对所有敌人施加末日
            foreach (Creature enemy in Owner.Creature.CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<DoomPower>(context, 
                    enemy,
                    doomAmount,
                    Owner.Creature,
                    null
                );
            }
        }
    }
}
