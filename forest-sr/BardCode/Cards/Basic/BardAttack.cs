using Forest_Sr.BardCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Threading.Tasks;
using static MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState;

namespace Forest_Sr.BardCode.Cards.Basic;
// 注册卡牌到诗人卡池
[RegisterCard(typeof(BardCardPool))]
public sealed class BardAttack : BardCard
{
    // 基础耗能
    private const int energyCost = 1;
    // 卡牌类型
    private const CardType type = CardType.Attack;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Basic;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.AnyEnemy;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;
    // 卡牌标签（Strike）
    protected override IEnumerable<string> RegisteredCardTagIds => new[] { "strike" };

    // 卡图资源
    //public override CardAssetProfile AssetProfile => new(
    //    PortraitPath: $"res://Test/images/cards/{GetType().Name}.png"
    // 卡框等，有需求自己添加。需要自行判断卡牌类型（攻击、技能、能力等）设置，建议写在基类里。
    // 如果使用自定义卡池，需要改下material（TODO）
    // FramePath: "", // 卡牌背景
    // PortraitBorderPath: "", // 边框（状态牌感染使用的）
    // BannerTexturePath: "" // 横幅（不同类型）
    //);

    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6, ValueProp.Move)
    ];

    public BardAttack() : base(energyCost, type, rarity, targetType)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(choiceContext);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}