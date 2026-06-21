using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 镜影术｜MirrorImage
/// 效果：获得 {mirror} 层镜像。每次被攻击时，减少 {reductionPerLayer}×层数的伤害，然后减少1层。
/// 升级：镜像层数 2 → 3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class MirrorImage : BardCard
{
    private const string _mirrorKey = "mirror";
    private const string _reductionKey = "reductionPerLayer";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_mirrorKey, 2),      // 镜像层数
        new DynamicVar(_reductionKey, 4)    // 每层减少的伤害
    ];

    // 关键词：魔法
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        BardKeywords.Magic
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<MirrorImagePower>()
    ];

    public MirrorImage() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：镜像层数 2 → 3
    protected override void OnUpgrade()
    {
        DynamicVars[_mirrorKey].UpgradeValueBy(1);
        // 每层减少伤害不变（仍然是4）
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int mirrorAmount = DynamicVars[_mirrorKey].IntValue;
        int reductionPerLayer = DynamicVars[_reductionKey].IntValue;

        // 施加镜像Power
        var power = await PowerCmd.Apply<MirrorImagePower>(choiceContext, 
            Owner.Creature,
            mirrorAmount,
            Owner.Creature,
            this
        );

        // 设置每层减少的伤害值
        power?.SetReductionPerLayer(reductionPerLayer);
    }
}
