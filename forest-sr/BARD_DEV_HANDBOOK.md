# BARD 项目开发手册（Wizard 角色参考）

> 从 BARD 诗人角色开发中积累的项目结构、模板、模型基准、常见陷阱。

---

## 一、项目结构

```
forest-sr/
├── Bard/                          # Godot 资源（图片/音频/场景/本地化）
│   ├── Audio/                     # MP3 音频 + FMOD bank (.bank, GUIDs.txt)
│   ├── Images/Cards/              # 卡牌图片（命名=类名.png）
│   ├── Images/Powers/             # 能力图标（命名=power_id小写下划线.png）
│   ├── Images/Powers/big/         # 大图标
│   ├── Images/Relics/             # 遗物图片
│   ├── Images/Charui/             # 角色UI（图标/能量/头像/map标记）
│   ├── Images/Visual/             # 角色模型图
│   ├── Scenes/                    # Godot .tscn 场景
│   ├── localization/
│   │   ├── zhs/                   # 中文
│   │   │   ├── cards.json
│   │   │   ├── powers.json
│   │   │   ├── relics.json
│   │   │   ├── card_keywords.json
│   │   │   ├── characters.json
│   │   │   ├── ancients.json
│   │   │   └── events.json
│   │   └── eng/                   # 英文（同结构）
│   └── MainFlies.cs               # 模组入口
│
└── BardCode/                      # C# 代码
    ├── Cards/
    │   ├── BardCards.cs           # 卡牌基类
    │   ├── KeyWord/BardKeyWords.cs # 关键词注册
    │   ├── Basic/                 # 基础
    │   ├── Common/                # 普通
    │   ├── Uncommon/              # 非普通
    │   ├── Rare/                  # 稀有
    │   ├── Ancient/               # 远古
    │   └── Other/                 # 状态/衍生
    ├── Powers/                    # 能力
    │   └── Counters/              # 隐藏计数器
    ├── Relics/                    # 遗物
    ├── Character/                 # 角色/卡池/遗物池
    ├── Extensions/                # 扩展方法
    ├── Patch/                     # Harmony 补丁
    └── Audio/BardAudio.cs         # 音频管理
```

---

## 二、卡牌模板

```csharp
using STS2RitsuLib.Interop.AutoRegistration;

[RegisterCard(typeof(WizardCardPool))]
public sealed class MyCard : WizardCard
{
    // 数值声明（自动差分显示）
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(9, ValueProp.Move),      // 伤害
        new BlockVar(6, ValueProp.Move),        // 格挡
        new PowerVar<WeakPower>(1),              // 层数
        new HealVar(8),                          // 回复
        new EnergyVar(1),                        // 能量
        new CardsVar(2),                         // 抽牌
        new RepeatVar(3),                        // 重复次数
        new DynamicVar("custom", 5)              // 自定义
    ];

    // 关键词（自动注入，本地化不需重复写）
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        WizardKeywords.Magic,           // 自定义
        CardKeyword.Exhaust             // 原版
    ];

    // 标签
    protected override HashSet<CardTag> CanonicalTags => new() { CardTag.Strike };

    // 悬停
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    // 费用/类型/稀有度/目标
    public MyCard() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    // 打出
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(ctx);
    }

    // 升级
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}
```

### 目标类型

`Self` | `AnyEnemy` | `AllEnemies` | `AnyPlayer` | `AllAllies` | `RandomEnemy`

### 伤害/格挡调用

```csharp
// 单体攻击
await DamageCmd.Attack(DynamicVars.Damage.IntValue)
    .FromCard(this).Targeting(cardPlay.Target!).Execute(ctx);

// 全体伤害
await CreatureCmd.Damage(ctx, CombatState!.HittableEnemies, amount, ValueProp.Move, Owner.Creature, this);

// 多段攻击
await DamageCmd.Attack(damage).WithHitCount(count).FromCard(this).Targeting(target).Execute(ctx);

// 获得格挡
await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(amount, ValueProp.Move), cardPlay);
```

---

## 三、卡牌模型基准

| 费用 | 普通 | 非普通 | 稀有 |
|------|------|--------|------|
| 0c | 单体4 / AoE3 / 效果 | — | — |
| 1c | 单体9 / 7-8+效果 / AoE8 | 单体9+效果 | 优质效果 |
| 2c | 单体~14 / AoE~11 | 单体12+效果 / AoE~18 | — |
| 3c | 单体~27 | 单体32 | 效果为主 |

- 多段攻击可略高（吃 buff 率高）
- debuff 占模则伤害降低
- 延迟收益（如吟唱）可偏高 20-30%

---

## 四、能力 (Power) 模板

```csharp
// 可见能力
[RegisterPower]
public sealed class MyPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;  // Single 或 Counter

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return;
        // ...
    }
}

// 隐藏计数器
public sealed class MyCounter : PowerModel
{
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override bool IsVisibleInternal => false;
}
```

### 常用钩子

`AfterCardPlayed` | `AfterPlayerTurnStart` | `AfterPlayerTurnStartEarly` | `BeforeHandDraw` | `ModifyHandDraw`

---

## 五、关键词注册

```csharp
[RegisterOwnedCardKeyword(nameof(Magic), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Chant))]
public class WizardKeywords
{
    public static readonly string MagicId = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(Magic));
    public static readonly CardKeyword Magic = MagicId.GetModCardKeyword();
    // ...
}
```

本地化 `card_keywords.json`:
```json
{
  "WIZARD_KEYWORD_MAGIC.title": "魔法",
  "WIZARD_KEYWORD_MAGIC.description": "..."
}
```

---

## 六、吟唱 (Chant) 延迟机制

```csharp
// 1. 定义 ChantPower
public sealed class FireBallChant : ChantPower
{
    public int DamageAmount { get; set; } = 14;
    protected override async Task OnChantTrigger(PlayerChoiceContext ctx)
    {
        await CreatureCmd.Damage(ctx, Owner!.CombatState!.HittableEnemies, DamageAmount, ValueProp.Move, Owner, null);
    }
}

// 2. 卡牌 OnPlay 中使用
var chant = await PowerCmd.Apply<FireBallChant>(ctx, Owner.Creature, 1, Owner.Creature, this);
chant.DamageAmount = DynamicVars.Damage.IntValue;
```

---

## 七、本地化 JSON

```json
{
  "WIZARD_CARD_MY_CARD.title": "卡牌名称",
  "WIZARD_CARD_MY_CARD.description": "造成{Damage:diff()}点伤害。\n施加{WeakPower:diff()}层[gold]虚弱[/gold]。"
}
```

### 规则

- 键名：`{MODID}_CARD_{CLASSNAME}` 全大写
- 关键词由 `CanonicalKeywords` 注入，**描述中不要重复写**
- `{Var:diff()}` 显示升级差分
- `\n` 分隔不同效果
- `{IfUpgraded:show:文本|}` 升级后才显示
- JSON 末项不能有尾随逗号 ⚠️

---

## 八、角色初始化 (`MainFile.cs`)

```csharp
[ModInitializer(nameof(Initialize))]
public class MainFile
{
    public const string ModId = "Wizard";

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);

        // 注册卡牌/遗物
        RitsuLibFramework.CreateContentPack(ModId)
            .Character<Wizard>()
            .CharacterStarterCard<Wizard, Strike>(4, order: 10)
            .CharacterStarterCard<Wizard, Defend>(4, order: 20)
            .CharacterStarterRelic<Wizard, StarterRelic>(1, order: 0)
            .Apply();

        // FMOD bank
        FmodStudioDeferredBankRegistration.RegisterBank("res://Wizard/Audio/Wizard.bank");
        FmodStudioDeferredBankRegistration.RegisterStudioGuidMappings("res://Wizard/Audio/GUIDs.txt");

        // 预加载音频
        AudioHelper.PreloadAll();

        // Harmony
        new Harmony(ModId).PatchAll(assembly);
    }
}
```

---

## 九、卡池 / 遗物池

```csharp
public sealed class WizardCardPool : TypeListCardPoolModel, IModColorfulPhilosophersCardPool
{
    public override string Title => "Wizard";
    public override string EnergyColorName => "wizard";    // 小写！
    public override Color DeckEntryCardColor => new("#...");
    public override bool IsColorless => false;

    // 可选：自定义卡框颜色
    private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateRgbShaderMaterial(r, g, b);
    public override Material? PoolFrameMaterial => _poolFrameMaterial;

    public override string? TextEnergyIconPath => "res://Wizard/Images/Charui/small_energy.png";
    public override string? BigEnergyIconPath => "res://Wizard/Images/Charui/big_energy.png";
}

public sealed class WizardRelicPool : TypeListRelicPoolModel
{
    public override string EnergyColorName => "Wizard";    // 大写！
}
```

---

## 十、Sea Glass / 色彩哲学家

- **Sea Glass**: `relics.json` → `"SEA_GLASS.WIZARD_CHARACTER_WIZARD.title": "..."`
- **色彩哲学家**: 卡池实现 `IModColorfulPhilosophersCardPool`
- **事件本地化** `events.json`:
```json
{
  "COLORFUL_PHILOSOPHERS.pages.INITIAL.options.WIZARD.title": "颜色名",
  "COLORFUL_PHILOSOPHERS.pages.INITIAL.options.WIZARD.description": "获得[blue]{Cards}[/blue]张卡牌。"
}
```

---

## 十一、角色类模板

```csharp
[RegisterCharacter]
public sealed class Wizard : ModCharacterTemplate<WizardCardPool, WizardRelicPool, SharedPotionPool>
{
    public const string CharacterId = "Wizard";
    public override Color NameColor => new("#...");
    public override int StartingHp => 72;
    public override int StartingGold => 99;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override float AttackAnimDelay => 0.25f;
    public override float CastAnimDelay => 0.25f;
    public override string CharacterSelectSfx => "event:/Wizard/sfx/...";
    public override bool AllowInVanillaRandomCharacterSelect => true;

    public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge(
        CharacterAssetProfiles.Ironclad(),
        new(Scenes: new(VisualsPath: "res://Wizard/Scenes/WizardVisual.tscn", ...),
             Ui: new(IconPath: "...", IconTexturePath: "...", ...))
    );
}
```

---

## 十二、常见陷阱

| 问题 | 原因 | 解决 |
|------|------|------|
| JSON 闪退 | 尾随逗号 `"text",}` | 最后一项不能有逗号 |
| 编译 CS1503 | `CardPileCmd.Add` 参数错 | 用 `(card, pile, pos, null, false)` |
| 空引用警告 | `Owner.CombatState` | 加 `!`: `Owner!.CombatState!` |
| 关键词重复显示 | 本地化写了关键词 | 删掉，`CanonicalKeywords` 自动注入 |
| 音效不触发 | `WithHitFx` 引用缺失文件 | 用 `BardAudio.PlayOneShot()` 或 `null` |
| 英文图鉴打不开 | 文件夹名错误 | 确认 `localization/eng/` |

---

## 十三、常用命令速查

```csharp
// Power 操作
await PowerCmd.Apply<StrengthPower>(ctx, target, amount, source, card);
await PowerCmd.ModifyAmount(ctx, power, delta, target, card, false);
await PowerCmd.Remove(power);

// 卡牌操作
await CardPileCmd.Draw(ctx, count, player);
await CardPileCmd.Add(card, pileType, position, null, false);
await CardPileCmd.Add(cards, pileType, position, null, false);     // List 重载
await CardPileCmd.AddGeneratedCardToCombat(card, pileType, player, position);

// 生物操作
await CreatureCmd.Damage(ctx, target, amount, valueProp, source, card);
await CreatureCmd.GainBlock(creature, blockVar, cardPlay);
await CreatureCmd.Heal(creature, amount);
await CreatureCmd.Stun(creature);
await CreatureCmd.TriggerAnim(creature, "Cast", delay);

// 玩家操作
await PlayerCmd.GainEnergy(amount, player);
```
