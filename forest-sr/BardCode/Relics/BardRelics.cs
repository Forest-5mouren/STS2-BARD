using Forest_Sr.BardCode.Extensions;
using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System;

namespace Forest_Sr.BardCode.Relics;

public abstract class BardRelics : ModRelicTemplate
{
    // ✅ 正确配置 AssetProfile
    public override RelicAssetProfile AssetProfile => new(
        IconPath: GetIconPath(),
        IconOutlinePath: GetIconOutlinePath(),
        BigIconPath: GetBigIconPath()
    );

    // 辅助方法：生成图标路径（注意添加 Bard/ 前缀）
    private string GetIconPath()
    {
        string path = $"res://Bard/Images/Relics/{GetImageName()}.png";
        return ResourceLoader.Exists(path) ? path : "res://Images/Relics/default_relic.png";
    }

    private string GetIconOutlinePath()
    {
        string path = $"res://Bard/Images/Relics/{GetImageName()}.png";
        return ResourceLoader.Exists(path) ? path : "res://Images/Relics/default_relic.png";
    }

    private string GetBigIconPath()
    {
        string path = $"res://Bard/Images/Relics/{GetImageName()}.png";
        return ResourceLoader.Exists(path) ? path : "res://Images/Relics/default_relic.png";
    }

    // 获取图片文件名
    private string GetImageName()
    {
        string id = Id.Entry;
        // 去掉 "BARD_RELIC_" 前缀，转为小写
        if (id.StartsWith("BARD_RELIC_"))
        {
            return id.Substring(11).ToLowerInvariant();
        }

        int lastUnderscore = id.LastIndexOf('_');
        if (lastUnderscore >= 0 && lastUnderscore < id.Length - 1)
        {
            return id.Substring(lastUnderscore + 1).ToLowerInvariant();
        }
        return id.ToLowerInvariant();
    }
}