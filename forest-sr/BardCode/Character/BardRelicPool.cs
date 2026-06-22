using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Relics;

public sealed class BardRelicPool : TypeListRelicPoolModel
{
    public override string EnergyColorName => "Bard";
    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://Bard/Images/Charui/small_energy.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://Bard/Images/Charui/big_energy.png";

}