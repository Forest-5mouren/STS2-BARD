using Forest_Sr.BardCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Forest_Sr.BardCode.Cards;

public sealed class BardCardPool : TypeListCardPoolModel
{
    public override string Title => "Bard";  // 卡池的ID。必须唯一防撞车。
    public override string EnergyColorName => "bard";
    //public override string CardFrameMaterialPath => "res://Bard/materials/card_frame.tres";
    public override Color DeckEntryCardColor => new("#840240");  // 卡池的主题色。
    // 能量表盘文字轮廓颜色
    public override Color EnergyOutlineColor => new(0.5f, 0.5f, 1f);
    // 如果你想用原版卡框换色，加这两行
    private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateRgbShaderMaterial(0.5f, 0.5f, 1f);
    public override Material? PoolFrameMaterial => _poolFrameMaterial;
    public override bool IsColorless => false; // 卡池是否是无色。例如事件、状态等卡池就是无色的。

    // 可选：自定义能量图标
    public override string? TextEnergyIconPath => "res://Bard/Images/Charui/small_energy.png";  // 描述中使用的能量图标。大小为24x24。
    public override string? BigEnergyIconPath => "res://Bard/Images/Charui/big_energy.png";  // tooltip和卡牌左上角的能量图标。大小为74x74
    
}