# 吟游诗人 Bard — Slay the Spire 2 Mod

基于 D&D 5e 规则的吟游诗人，为 *Slay the Spire 2* 增添了一位全新可玩角色。

依赖 [RitsuLib](https://github.com/BAKAOLC/STS2-RitsuLib) 制作。

## 总览

| 项目 | 数量 |
|------|------|
| 卡牌总数 | **86 张** |
| 遗物 | 4 个 |
| 关键词 | 3 个（魔法 / 乐曲 / 吟唱） |
| 本地化 | 简体中文 · English |

## 关键词

| 关键词 | 效果 |
|--------|------|
| **魔法** (Magic) | 吟游诗人吹奏乐曲来引动魔法力量 |
| **乐曲** (Song) | 吟游诗人谱写的小曲，不需要魔法，仅凭声音发挥音乐的力量 |
| **吟唱** (Chant) | 效果延迟至下一回合开始时自动生效，不会因受到伤害而中断 |

## 核心机制

**和声 (Harmony)** — 初始遗物提供。每打出魔法牌或乐曲牌，和声+1。回合开始时全体获得和声层数的活力，然后清空。
**活力 (Vigor)** — 诗人的主要战斗资源，可通过乐曲牌累积，用于增幅多种效果。

## 卡牌列表

### Basic（基础卡）

| 卡牌 | 费用 | 类型 | 关键词 | 效果 |
|------|:---:|:----:|--------|------|
| Shrill Scream / 刺耳尖叫 | 1 | Attack | Song, Strike | 造成伤害，施加易伤 |
| Harmony Shield / 和谐之盾 | 1 | Skill | Song, Defend | 获得格挡和临时敏捷 |
| Vicious Mockery / 恶言相加 | 1 | Attack | — | 敌人失去生命，施加虚弱 |
| Blade Ward / 剑刃防护 | 1 | Skill | Magic | 获得格挡，虚弱敌人伤害减半 |

### Common（罕见）

| 卡牌 | 费用 | 类型 | 关键词 | 效果 |
|------|:---:|:----:|--------|------|
| Bane / 灾祸术 | 1 | Skill | Magic | 全体敌人施加虚弱 |
| Song of Courage / 勇气之歌 | 1 | Skill | Song, Chant | 吟唱。下回合获得力量+活力 |
| Brisk Beat / 轻快节拍 | 1 | Power | Magic | 全体友方每回合抽1张牌（持续数回合） |
| Crippling Thrust / 致残突刺 | 1 | Attack | — | 伤害，施加虚弱+易伤 |
| Disengage / 脱离 | 1 | Skill | — | 格挡。上张攻击则抽2牌，上张技能则额外格挡 |
| Dissonant Whispers / 不谐低语 | 2 | Skill | Magic | 全体敌人混乱伤害 |
| Emergency / 应急 | 1 | Attack | — | 伤害。下张魔法或乐曲牌费用-1 |
| Heroism / 英雄气概 | 1 | Skill | Magic | 获得英雄气概：每回合开始重新获得格挡 |
| Honeyed Tongue / 巧舌如簧 | 1 | Skill | — | 格挡+全体易伤。上张技能则改为全体虚弱 |
| Mage Hand / 法师之手 | 1 | Skill | Magic | 抽牌，手牌中若干张获得保留 |
| Raging Flurry / 狂怒连击 | 1 | Attack | — | 造成多次伤害 |
| Rhythm Strike / 韵律打击 | 1 | Attack | Song | 伤害。和声>3时获得能量+抽牌 |
| Slide Step / 滑步 | 1 | Skill | — | 获得临时敏捷 |
| Thunder Clap / 鸣雷破 | 2 | Attack | Magic | 全体伤害 |
| Thunderwave / 雷鸣波 | 1 | Attack | Song | 全体伤害+击退效果 |
| Valiant Strike / 英勇打击 | 1 | Attack | Song | 伤害，获得活力 |
| Weakpoint Strike / 弱点刺击 | 1 | Attack | — | 伤害，寻找弱点 |

### Uncommon（稀有）

| 卡牌 | 费用 | 类型 | 关键词 | 效果 |
|------|:---:|:----:|--------|------|
| Allegro / 快板 | 1 | Power | Magic | 全体友方每回合获得1能量（持续数回合） |
| All-Night Composition / 通宵谱曲 | 0 | Skill | — | 获得能量，往抽牌堆加入1张晕眩 |
| Banshee Wail / 女妖之嚎 | 0 | Skill | Song, Chant, Exhaust | 吟唱。下回合全体敌人虚弱+易伤 |
| Boost Morale / 提振士气 | 1 | Skill | — | 全体格挡。消耗活力获得额外格挡 |
| Bottoms Up / 把酒迎欢 | 1 | Skill | — | 获得力量+活力，往弃牌堆加入晕眩 |
| Build Up Momentum / 蓄势待发 | 1 | Skill | — | 获得活力，本回合手牌保留 |
| Clear Chant / 清晰赞歌 | 0 | Skill | Song, Chant | 吟唱。下回合搜索魔法牌加入手牌，获得易伤 |
| Cure Wounds / 疗伤术 | 1 | Skill | Magic | 回复友方生命 |
| Deadly Performance / 夺命演奏 | 1 | Power | — | 能力。释放魔法时对随机敌人造成伤害 |
| Ear-Piercing Shriek / 尖锐刺耳 | 1 | Attack | Song | 全体伤害+虚弱 |
| Enhance Ability / 强化属性 | 1 | Skill | Magic | 选择获得：力量/敏捷/抽牌+减费 |
| Enlarge / 变巨术 | 1 | Power | Magic | 获得力量 |
| Explosive Power / 爆发力 | 1 | Skill | — | 消耗活力，从抽牌堆各抽1张攻击/技能/能力 |
| Faerie Fire / 妖火 | 1 | Skill | Magic | 全体敌人施加易伤 |
| Fireball / 火球术 | 3 | Attack | Magic | 全体高额伤害 |
| Flight of the Bumblebee / 野蜂飞翔 | 1 | Attack | Magic | 随机目标多次伤害 |
| Flurry Thrust / 连续突刺 | 1 | Attack | — | 根据本回合打出牌类型追加伤害 |
| Follow-Up Strike / 顺势戳击 | 1 | Attack | — | 上张攻击或技能则获得额外效果 |
| Guardian Rune / 守卫刻文 | 2 | Power | — | 能力。失去格挡时对全体造成等量伤害 |
| Hearsay / 道听途说 | 0 | Skill | — | 随机获得一张乐曲或魔法牌，本回合0费 |
| Improvise / 即兴 | 0 | Power | — | 能力。每回合随机获得一首乐曲（排除吟唱） |
| Inspiration Resonance / 灵感共鸣 | 0 | Skill | — | 本回合每张乐曲牌回复能量。虚无 |
| Inspiring Melody / 激昂旋律 | 2 | Skill | Song, Chant | 吟唱。下回合全体队友力量+敏捷 |
| Longstrider / 大步奔行 | 1 | Skill | Magic | 目标获得敏捷，抽牌 |
| Magical Armor / 魔法护甲 | 1 | Skill | Magic | 获得覆甲 |
| Melodic Ward / 旋律护身 | 1 | Power | — | 能力。释放魔法时获得格挡 |
| Parry Riposte / 格挡反击 | 1 | Attack | — | 伤害。上张技能则回复能量 |
| Polymorph / 变形术 | 1 | Skill | Magic | 将手牌中一张牌变为随机魔法牌 |
| Reduce / 缩小术 | 1 | Skill | Magic | 降低敌人力量 |
| Rhapsody / 狂想曲 | 1 | Skill | — | 抽牌。抽到的每张魔法牌回复能量 |
| See You Again / See You Again | 1 | Skill | — | 从弃牌堆回收牌，然后丢弃1张 |
| Serene Song / 宁静之歌 | 2 | Skill | Song, Chant | 吟唱。下回合全体队友回复生命 |
| Sword Dance / 剑舞 | 1 | Attack | — | 伤害。上张攻击则攻击2次，上张技能则得格挡 |
| Thunderclap Blade / 轰雷剑 | 1 | Attack | Magic | 伤害。目标本回合获得负面状态时追加伤害 |
| True Strike / 克敌机先 | 1 | Attack | Magic | 去除敌人所有人工制品，施加易伤 |
| Valiant Dash / 英勇冲刺 | 1 | Skill | — | 消耗活力抽牌 |
| War Song / 战歌 | 1 | Power | — | 能力。每张乐曲牌获得活力 |
| Weakness Shriek / 虚弱尖叫 | 1 | Skill | Song, Exhaust | 全体降低力量+敏捷，消耗 |

### Rare（极稀有）

| 卡牌 | 费用 | 类型 | 关键词 | 效果 |
|------|:---:|:----:|--------|------|
| Arcane Echo / 奥术回响 | 1 | Power | Magic | 能力。和声达阈值时从消耗堆回收一张魔法牌 |
| Bestow Curse / 降咒 | X | Attack | Magic | X费。造成X次伤害，施加虚弱，降低力量 |
| Calm Emotions / 安定心神 | 1 | Skill | Magic | 清除目标所有负面效果 |
| Curtain Call / 收场 | 2 | Attack | — | 伤害。本场战斗每使用过一种卡牌类型攻击一次 |
| Dancing Youth / 舞动青春 | 1 | Power | — | 能力。回合结束和声>3时下回合抽牌 |
| Dark Cacophony / 黑暗喧嚣 | 1 | Power | Song | 能力。每回合开始全体失去生命 |
| Echoing Melody / 余音绕梁 | 1 | Power | — | 能力。每回合复制上一张乐曲牌（带消耗） |
| Encore / 安可 | 2 | Attack | Song | 伤害。本场战斗每打出过一张乐曲牌攻击一次 |
| Eyebite / 摄心目光 | 2 | Skill | Magic | 眩晕敌人，施加虚弱，降低力量 |
| Haste / 加速术 | 1 | Skill | Magic | 2回合内每回合+2能量+2抽牌，结束后获得易伤+虚弱 |
| Invisibility / 隐身术 | 1 | Skill | Magic | 友方获得无实体 |
| Magic Weapon / 魔法武器 | 1 | Power | — | 获得力量。非魔法攻击牌获得魔法词条 |
| Mana Surge / 法力充沛 | 1 | Power | — | 能力。每回合首次释放魔法时抽牌 |
| Mass Polymorph / 群体变形术 | 1 | Skill | Magic | 选择手牌中任意张变为随机魔法牌 |
| Mirror Image / 镜影术 | 1 | Power | Magic | 获得镜像：被攻击时按层数减伤 |
| Never Give Up / 绝不认输 | 2 | Power | — | 即将死亡时回复生命。本回合每张乐曲牌费用-1 |
| Never Gonna Give You Up | 1 | Skill | — | 从消耗堆回收牌 |
| Overclock Casting / 超频施法 | 1 | Power | — | 能力。魔法牌获得消耗和再次打出 |
| Peerless Dance / 无双华舞 | 1 | Skill | — | 手牌中一张攻击牌获得重放 |
| Prismatic Spray / 虹光喷射 | 3 | Attack | Magic | 对单体造成多次伤害 |
| Street Performance / 街头卖艺 | 0 | Skill | — | 本回合演奏过乐曲则获得金币 |
| Valiant Presence / 英勇气势 | 1 | Power | — | 能力。失去活力时获得格挡 |

### Ancient（远古 — 通过远古遗物事件获得）

| 卡牌 | 费用 | 类型 | 关键词 | 效果 |
|------|:---:|:----:|--------|------|
| Phantasmal Killer / 魅影杀手 | 1 | Attack | Magic | 伤害+虚弱。打出后下回合回手 |
| Rhythm Storm / 韵律风暴 | 1 | Power | Magic, Song | 能力。乐曲/魔法获得活力，获得活力时全体伤害 |

### Other（其他）

| 卡牌 | 费用 | 类型 | 关键词 | 效果 |
|------|:---:|:----:|--------|------|
| Bull Strength / 牛之力量 | 1 | Power | Magic | 获得力量 |
| Cat Grace / 猫之敏捷 | 1 | Skill | Magic | 获得敏捷 |
| Fox Cunning / 狐之狡黠 | 1 | Skill | Magic | 抽牌，下张魔法牌费用-1 |

## 遗物

| 遗物 | 效果 |
|------|------|
| **诗人激励**（初始） | 每打出魔法/乐曲牌，和声+1。回合开始全体获得对应活力，和声清零 |
| **诗人激励+**（升级） | 同上，但和声调整为 2（不清零） |
| **唤蛇长笛** | 第一回合将蛇咬+加入手牌，使其获得乐曲 |
| **阿尔图罗的小提琴** | 演奏乐曲时，给与敌方全体 2 层灾厄 |

## 安装

1. 确保已安装 [RitsuLib](https://github.com/BAKAOLC/STS2-RitsuLib)
2. 将  forest-sr/  目录放入  Slay the Spire 2/mods/

## 技术栈

- **Godot 4.5.1 Mono**（C#）
- **RitsuLib** 模组框架
- **Slay the Spire 2**

## 鸣谢

- D&D 5e 规则设定
- [RitsuLib](https://github.com/BAKAOLC/STS2-RitsuLib) 模组框架
- *Slay the Spire 2* 模组社区
