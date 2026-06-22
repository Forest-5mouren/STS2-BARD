using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards;

[RegisterCard(typeof(BardCardPool), Inherit = true)]
public abstract class BardCard : ModCardTemplate
{
    protected BardCard(int cost, CardType type, CardRarity rarity, TargetType target)
        : base(cost, type, rarity, target)
    {
    }

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Bard/Images/Cards/{GetType().Name}.png"

    // 根据不同类型设置不同卡框
    //FramePath: type switch
    //{
    //    CardType.Attack => "res://RitsuTest/images/card_frame_attack.png",
    //    CardType.Skill => "res://RitsuTest/images/card_frame_skill.png",
    //    CardType.Power => "res://RitsuTest/images/card_frame_power.png",
    //    _ => ""
    //}
    // PortraitBorderPath: "",
    // BannerTexturePath: ""
    );
}