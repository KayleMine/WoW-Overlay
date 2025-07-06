using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kibitka.box.DB
{
    public class Name_And_TextureName
    {
        public string Name;
        public string TextureName;

        public Name_And_TextureName()
        { }

        public Name_And_TextureName(string Name, string textureName)
        {
            this.Name = Name;
            this.TextureName = textureName;
        }
    }
    public class DB
    {





        private static string GetName(string ruName, string enName) => CultureInfo.CurrentCulture.Name.StartsWith("ru") ? ruName : enName;




        #region Руды!
        private static readonly IDictionary<uint, Name_And_TextureName> OresDB =
        new (uint id, string ruName, string enName, string texture)[]
        {
            (1731,   "Медная жила", "Copper ore", "mine_copper"),
            (3763,   "Медная жила", "Copper ore", "mine_copper"),
            (2055,   "Медная жила", "Copper ore", "mine_copper"),
            (181248, "Медная жила", "Copper ore", "mine_copper"),
            (103713, "Медная жила", "Copper ore", "mine_copper"),
            (1610,   "Ароматитовая жила", "Incendicite Mineral Vein", "mine_blood_iron"),
            (1667,   "Ароматитовая жила", "Incendicite Mineral Vein", "mine_blood_iron"),
            (1732,   "Оловянная жила", "Tin ore", "mine_tin"),
            (3764,   "Оловянная жила","Tin ore", "mine_tin"),
            (2054,   "Оловянная жила","Tin ore", "mine_tin"),
            (181249, "Оловянная жила","Tin ore", "mine_tin"),
            (103711, "Оловянная жила","Tin ore", "mine_tin"),
            (2653,   "Малое месторождение кровавого камня", "Lesser Bloodstone Deposit", "mine_blood_iron"),
            (73940,  "Покрытая слизью серебряная жила", "Ooze Covered Silver Vein", "mine_silver"),
            (1733,   "Серебряная жила", "Silver Vein", "mine_silver"),
            (105569, "Серебряная жила", "Silver Vein", "mine_silver"),
            (1735,   "Залежи железа", "Iron Deposit", "mine_iron"),
            (19903,  "Индарилиевая жила", "Indurium Mineral Vein", "mine_indurium"),
            (1734,   "Золотая жила", "Gold Vein", "mine_gold"),
            (150080, "Золотая жила", "Gold Vein", "mine_gold"),
            (181109, "Золотая жила", "Gold Vein", "mine_gold"),
            (73941,  "Покрытая слизью золотая жила", "Gold Vein", "mine_gold"),
            (2040,   "Мифриловые залежи", "Mithril Deposit", "mine_mithril"),
            (150079, "Мифриловые залежи", "Mithril Deposit", "mine_mithril"),
            (176645, "Мифриловые залежи", "Mithril Deposit", "mine_mithril"),
            (123310, "Покрытые слизью мифриловые залежи", "Ooze Covered Mithril Deposit", "mine_mithril"),
            (2047,   "Залежи истинного серебра", "Truesilver Deposit", "mine_truesilver"),
            (181108, "Залежи истинного серебра", "Truesilver Deposit", "mine_truesilver"),
            (150081, "Залежи истинного серебра", "Truesilver Deposit", "mine_truesilver"),
            (165658, "Залежи черного железа", "Dark Iron Deposit", "mine_darkiron"),
            (123848, "Покрытая слизью ториевая жила", "Ooze Covered Thorium Vein", "mine_thorium"),
            (324,    "Малая ториевая жила", "Small Thorium Vein", "mine_thorium"),
            (150082, "Малая ториевая жила", "Small Thorium Vein", "mine_thorium"),
            (176643, "Малая ториевая жила", "Small Thorium Vein", "mine_thorium"),
            (180215, "Ториевая жила Хаккари", "Hakkari Thorium Vein", "mine_thorium"),
            (177388, "Покрытая слизью богатая ториевая жила", "Ooze Covered Rich Thorium Vein", "mine_rich_thorium"),
            (175404, "Богатая ториевая жила", "Rich Thorium Vein", "mine_rich_thorium"),
            (181555, "Месторождение оскверненного железа", "Fel Iron Deposit", "mine_feliron"),
            (185877, "Месторождение Адамантита", "Nethercite Deposit", "mine_adamantium"),
            (181069, "Большая обсидиановая глыба", "Large Obsidian Chunk", "Obsidian"),
            (181068, "Маленький кусочек обсидиана", "Small Obsidian Chunk", "Obsidian"),
            (181556, "Залежи адамантита", "Adamantite Deposit", "mine_adamantium"),
            (189978, "Залежи кобальта", "Cobalt Deposit", "mine_cobalt"),
            (181569, "Богатые залежи адамантита", "Rich Adamantite Deposit", "mine_rich_adamantium"),
            (181570, "Богатые залежи адамантита", "Rich Adamantite Deposit", "mine_rich_adamantium"),
            (185557, "Древняя самоцветная жила",  "Ancient Gem Vein", "ancient_gem"),
            (181557, "Кориевая жила", "Khorium Vein", "mine_khorium"),
            (189979, "Богатые залежи кобальта", "Rich Cobalt Deposit", "mine_cobalt"),
            (189980, "Месторождение саронита", "Saronite Deposit", "mine_saronite"),
            (189981, "Богатое месторождение саронита", "Rich Saronite Deposit", "mine_saronite"),
            (195036, "Месторождение чистого саронита", "Pure Saronite Deposit", "mine_saronite"),
            (191133, "Залежи титана", "Titanium Vein", "mine_titanium"),

        }.ToDictionary(
        x => x.id,
        x => new Name_And_TextureName(GetName(x.ruName, x.enName), x.texture)
        );
        #endregion



        #region Травы!
        private static readonly IDictionary<uint, Name_And_TextureName> HerbsDB =
        new (uint id, string ruName, string enName, string texture)[]
        {
            (181166, "Кровопийка", "Bloodthistle",   "herb_bloodthistle"),
            (1618,   "Мироцвет", "Peacebloom",     "herb_peacebloom"),
            (3724,   "Мироцвет", "Peacebloom",     "herb_peacebloom"),
            (1617, "Сребролист", "Silverleaf", "herb_silverleaf"),
            (3725, "Сребролист", "Silverleaf", "herb_silverleaf"),
            (1619, "Земляной корень", "Earthroot", "herb_earthroot"),
            (3726, "Земляной корень", "Earthroot", "herb_earthroot"),
            (1620, "Магороза", "Mageroyal", "herb_mageroyal"),
            (3727, "Магороза", "Mageroyal", "herb_mageroyal"),
            (1621, "Остротерн", "Briarthorn", "herb_briarthorn"),
            (3729, "Остротерн", "Briarthorn", "herb_briarthorn"),
            (2045, "Удавник", "Stranglekelp", "herb_stranglekelp"),
            (1622, "Синячник", "Bruiseweed", "herb_bruiseweed"),
            (3730, "Синячник", "Bruiseweed", "herb_bruiseweed"),
            (1623, "Дикий сталецвет", "Wild Steelbloom", "herb_wild_steelbloom"),
            (1628, "Могильный мох", "Grave Moss", "herb_grave_moss"),
            (1624, "Королевская кровь", "Kingsblood", "herb_kingsblood"),
            (2041, "Корень жизни", "Liferoot", "herb_liferoot"),
            (2042, "Бледнолист", "Fadeleaf", "herb_fadeleaf"),
            (2046, "Златошип", "Goldthorn", "herb_goldthorn"),
            (2043, "Кадгаров ус", "Khadgars Whisker", "herb_khadgars_whisker"),
            (2044, "Морозник", "Wintersbite", "herb_wintersbite"),
            (2866, "Огнецвет", "Firebloom", "herb_firebloom"),
            (142140, "Лиловый лотос", "Purple Lotus", "herb_purple_lotus"),
            (180165, "Лиловый лотос", "Purple Lotus", "herb_purple_lotus"),
            (142141, "Слезы Артаса", "Arthas Tears", "herb_arthas_tears"),
            (176642, "Слезы Артаса", "Arthas Tears", "herb_arthas_tears"),
            (142142, "Солнечник", "Sungrass", "herb_sungrass"),
            (176636, "Солнечник", "Purple Lotus", "herb_sungrass"),
            (180164, "Солнечник", "Purple Lotus", "herb_sungrass"),
            (142143, "Пастушья сумка", "Blindweed", "herb_blindweed"),
            (183046, "Пастушья сумка", "Blindweed", "herb_blindweed"),
            (142144, "Призрачная поганка", "Ghost Mushroom", "herb_ghost_mushroom"),
            (142145, "Кровь Грома", "Gromsblood", "herb_gromsblood"),
            (176637, "Кровь Грома", "Ggromsblood", "herb_gromsblood"),
            (176583, "Золотой сансам", "Golden Sansam", "herb_golden_sansam"),
            (176638, "Золотой сансам", "Golden Sansam", "herb_golden_sansam"),
            (180167, "Золотой сансам", "Golden Sansam", "herb_golden_sansam"),
            (176584, "Снолист", "Dreamfoil", "herb_dreamfoil"),
            (176639, "Снолист", "Dreamfoil", "herb_dreamfoil"),
            (180168, "Снолист", "Dreamfoil", "herb_dreamfoil"),
            (176586, "Горный серебряный шалфей", "Mountain Silversage", "herb_mountain_silversage"),
            (176640, "Горный серебряный шалфей", "Mountain Silversage", "herb_mountain_silversage"),
            (180166, "Горный серебряный шалфей", "Mountain Silversage", "herb_mountain_silversage"),
            (176587, "Чумоцвет", "Plaguebloom", "herb_plaguebloom"),
            (176641, "Чумоцвет", "Plaguebloom", "herb_plaguebloom"),
            (176588, "Ледяной зев", "Icecap", "herb_icecap"),
            (176589, "Черный лотос", "Black Lotus", "herb_black_lotus"),
            (181270, "Сквернопля", "Felweed", "herb_felweed"),
            (183044, "Сквернопля", "Felweed", "herb_felweed"),
            (190174, "Мерзлая трава", "Frozen Herb", "Frozen_Herb"),
            (181271, "Сияние грез", "Dreaming Glory", "herb_dreaming_glory"),
            (183045, "Сияние грез", "Dreaming Glory", "herb_dreaming_glory"),
            (183043, "Кисейница", "Ragveil", "herb_ragveil"),
            (181275, "Кисейница", "Ragveil", "herb_ragveil"),
            (181277, "Терошишка", "Terocone", "herb_terocone"),
            (181276, "Огненный зев", "Flame Cap", "herb_flame_cap"),
            (181278, "Древний лишайник", "Ancient Lichen", "herb_ancient_lichen"),
            (189973, "Золотой клевер", "Goldclover", "herb_goldclover"),
            (181279, "Пустоцвет", "Netherbloom", "herb_netherbloom"),
            (185881, "Куст пустопраха", "Netherdust", "herb_netherdust"),
            (191303, "Огница", "Firethorn", "herb_firethorn"),
            (181280, "Ползучий кошмарник", "Nightmare Vine", "herb_nightmare_vine"),
            (181281, "Манаполох", "Mana Thistle", "herb_mana_thistle"),
            (190169, "Тигровая лилия", "Tigerlily", "herb_tigerlily"),
            (190170, "Роза Таландры", "Talandras Rose", "herb_trose"),
            (191019, "Язык аспида", "Adder Tongue", "Adder_tongue"),
            (190173, "Мерзлая трава", "Frozen Herb", "Frozen_Herb"),
            (190175, "Мерзлая трава", "Frozen Herb", "Frozen_Herb"),
            (190171, "Личецвет", "Lichbloom", "herb_lichbloom"),
            (190172, "Ледошип", "Icethorn", "herb_icethorn"),
            (190176, "Северный лотос", "Frostlotus", "herb_frostlotus"),

        }.ToDictionary(
        x => x.id,
        x => new Name_And_TextureName(GetName(x.ruName, x.enName), x.texture)
        );
        #endregion


        #region Рарники!
        private static IDictionary<UInt32, Name_And_TextureName> RareNpcDB = new Dictionary<UInt32, Name_And_TextureName>()
            {
                {26459, new Name_And_TextureName("test NPC", "rare")},
                {179487, new Name_And_TextureName("Rare NPC", "rare")},
                {32517, new Name_And_TextureName("Rare NPC", "rare")},
                {35189, new Name_And_TextureName("Rare NPC", "rare")},
                {38453, new Name_And_TextureName("Rare NPC", "rare")},
                {33776, new Name_And_TextureName("Rare NPC", "rare")},
                {32485, new Name_And_TextureName("Rare NPC", "rare")},
                {32630, new Name_And_TextureName("Rare NPC", "rare")},
                {32491, new Name_And_TextureName("Rare NPC", "rare")},
                {32500, new Name_And_TextureName("Rare NPC", "rare")},
                {32481, new Name_And_TextureName("Rare NPC", "rare")},
                {32398, new Name_And_TextureName("Rare NPC", "rare")},
                {5828, new Name_And_TextureName("Rare NPC", "rare")},
                {32361, new Name_And_TextureName("Rare NPC", "rare")},
                {32501, new Name_And_TextureName("Rare NPC", "rare")},
                {32429, new Name_And_TextureName("Rare NPC", "rare")},
                {32358, new Name_And_TextureName("Rare NPC", "rare")},
                {32471, new Name_And_TextureName("Rare NPC", "rare")},
                {32377, new Name_And_TextureName("Rare NPC", "rare")},
                {32417, new Name_And_TextureName("Rare NPC", "rare")},
                {32475, new Name_And_TextureName("Rare NPC", "rare")},
                {32400, new Name_And_TextureName("Rare NPC", "rare")},
                {32487, new Name_And_TextureName("Rare NPC", "rare")},
                {32422, new Name_And_TextureName("Rare NPC", "rare")},
                {521, new Name_And_TextureName("Rare NPC", "rare")},
                {32357, new Name_And_TextureName("Rare NPC", "rare")},
                {32438, new Name_And_TextureName("Rare NPC", "rare")},
                {32386, new Name_And_TextureName("Rare NPC", "rare")},
                {20932, new Name_And_TextureName("Rare NPC", "rare")},
                {32495, new Name_And_TextureName("Rare NPC", "rare")},
                {32409, new Name_And_TextureName("Rare NPC", "rare")},
                {3253, new Name_And_TextureName("Rare NPC", "rare")},
                {1130, new Name_And_TextureName("Rare NPC", "rare")},
                {32447, new Name_And_TextureName("Rare NPC", "rare")},
                {11498, new Name_And_TextureName("Rare NPC", "rare")},
                {2172, new Name_And_TextureName("Rare NPC", "rare")},
                {14339, new Name_And_TextureName("Rare NPC", "rare")},
                {7057, new Name_And_TextureName("Rare NPC", "rare")},
                {18678, new Name_And_TextureName("Rare NPC", "rare")},
                {9024, new Name_And_TextureName("Rare NPC", "rare")},
                {2850, new Name_And_TextureName("Rare NPC", "rare")},
                {5356, new Name_And_TextureName("Rare NPC", "rare")},
                {18698, new Name_And_TextureName("Rare NPC", "rare")},
                {11497, new Name_And_TextureName("Rare NPC", "rare")},
                {4132, new Name_And_TextureName("Rare NPC", "rare")},
                {2447, new Name_And_TextureName("Rare NPC", "rare")},
                {6584, new Name_And_TextureName("Rare NPC", "rare")},
                {10826, new Name_And_TextureName("Rare NPC", "rare")},
                {2753, new Name_And_TextureName("Rare NPC", "rare")},
                {3535, new Name_And_TextureName("Rare NPC", "rare")},
                {1920, new Name_And_TextureName("Rare NPC", "rare")},
                {17144, new Name_And_TextureName("Rare NPC", "rare")},
                {14477, new Name_And_TextureName("Rare NPC", "rare")},
                {8208, new Name_And_TextureName("Rare NPC", "rare")},
                {2931, new Name_And_TextureName("Rare NPC", "rare")},
                {12431, new Name_And_TextureName("Rare NPC", "rare")},
                {14343, new Name_And_TextureName("Rare NPC", "rare")},
                {18696, new Name_And_TextureName("Rare NPC", "rare")},
                {10200, new Name_And_TextureName("Rare NPC", "rare")},
                {10119, new Name_And_TextureName("Rare NPC", "rare")},
                {18677, new Name_And_TextureName("Rare NPC", "rare")},
                {5807, new Name_And_TextureName("Rare NPC", "rare")},
                {18683, new Name_And_TextureName("Rare NPC", "rare")},
                {18679, new Name_And_TextureName("Rare NPC", "rare")},
                {6585, new Name_And_TextureName("Rare NPC", "rare")},
                {14471, new Name_And_TextureName("Rare NPC", "rare")},
                {3672, new Name_And_TextureName("Rare NPC", "rare")},
                {2754, new Name_And_TextureName("Rare NPC", "rare")},
                {3056, new Name_And_TextureName("Rare NPC", "rare")},
                {12237, new Name_And_TextureName("Rare NPC", "rare")},
                {22060, new Name_And_TextureName("Rare NPC", "rare")},
                {5863, new Name_And_TextureName("Rare NPC", "rare")},
                {18680, new Name_And_TextureName("Rare NPC", "rare")},
                {14269, new Name_And_TextureName("Rare NPC", "rare")},
                {14491, new Name_And_TextureName("Rare NPC", "rare")},
                {2779, new Name_And_TextureName("Rare NPC", "rare")},
                {462, new Name_And_TextureName("Rare NPC", "rare")},
                {3652, new Name_And_TextureName("Rare NPC", "rare")},
                {14280, new Name_And_TextureName("Rare NPC", "rare")},
                {18694, new Name_And_TextureName("Rare NPC", "rare")},
                {2606, new Name_And_TextureName("Rare NPC", "rare")},
                {6488, new Name_And_TextureName("Rare NPC", "rare")},
                {1260, new Name_And_TextureName("Rare NPC", "rare")},
                {10644, new Name_And_TextureName("Rare NPC", "rare")},
                {14237, new Name_And_TextureName("Rare NPC", "rare")},
                {2751, new Name_And_TextureName("Rare NPC", "rare")},
                {6490, new Name_And_TextureName("Rare NPC", "rare")},
                {5823, new Name_And_TextureName("Rare NPC", "rare")},
                {32435, new Name_And_TextureName("Rare NPC", "rare")},
                {5865, new Name_And_TextureName("Rare NPC", "rare")},
                {14430, new Name_And_TextureName("Rare NPC", "rare")},
                {12432, new Name_And_TextureName("Rare NPC", "rare")},
                {3581, new Name_And_TextureName("Rare NPC", "rare")},
                {2175, new Name_And_TextureName("Rare NPC", "rare")},
                {1851, new Name_And_TextureName("Rare NPC", "rare")},
                {16380, new Name_And_TextureName("Rare NPC", "rare")},
                {14232, new Name_And_TextureName("Rare NPC", "rare")},
                {2749, new Name_And_TextureName("Rare NPC", "rare")},
                {8299, new Name_And_TextureName("Rare NPC", "rare")},
                {12037, new Name_And_TextureName("Rare NPC", "rare")},
                {9041, new Name_And_TextureName("Rare NPC", "rare")},
                {14222, new Name_And_TextureName("Rare NPC", "rare")},
                {573, new Name_And_TextureName("Rare NPC", "rare")},
                {8207, new Name_And_TextureName("Rare NPC", "rare")},
                {3068, new Name_And_TextureName("Rare NPC", "rare")},
                {5830, new Name_And_TextureName("Rare NPC", "rare")},
                {599, new Name_And_TextureName("Rare NPC", "rare")},
                {14224, new Name_And_TextureName("Rare NPC", "rare")},
                {18682, new Name_And_TextureName("Rare NPC", "rare")},
                {6582, new Name_And_TextureName("Rare NPC", "rare")},
                {2752, new Name_And_TextureName("Rare NPC", "rare")},
                {8204, new Name_And_TextureName("Rare NPC", "rare")},
                {1132, new Name_And_TextureName("Rare NPC", "rare")},
                {5824, new Name_And_TextureName("Rare NPC", "rare")},
                {8300, new Name_And_TextureName("Rare NPC", "rare")},
                {5842, new Name_And_TextureName("Rare NPC", "rare")},
                {4425, new Name_And_TextureName("Rare NPC", "rare")},
                {5931, new Name_And_TextureName("Rare NPC", "rare")},
                {14234, new Name_And_TextureName("Rare NPC", "rare")},
                {574, new Name_And_TextureName("Rare NPC", "rare")},
                {10081, new Name_And_TextureName("Rare NPC", "rare")},
                {18241, new Name_And_TextureName("Rare NPC", "rare")},
                {10077, new Name_And_TextureName("Rare NPC", "rare")},
                {18686, new Name_And_TextureName("Rare NPC", "rare")},
                {8200, new Name_And_TextureName("Rare NPC", "rare")},
                {8211, new Name_And_TextureName("Rare NPC", "rare")},
                {13896, new Name_And_TextureName("Rare NPC", "rare")},
                {9046, new Name_And_TextureName("Rare NPC", "rare")},
                {519, new Name_And_TextureName("Rare NPC", "rare")},
                {596, new Name_And_TextureName("Rare NPC", "rare")},
                {10558, new Name_And_TextureName("Rare NPC", "rare")},
                {5797, new Name_And_TextureName("Rare NPC", "rare")},
                {8205, new Name_And_TextureName("Rare NPC", "rare")},
                {14697, new Name_And_TextureName("Rare NPC", "rare")},
                {8219, new Name_And_TextureName("Rare NPC", "rare")},
                {2598, new Name_And_TextureName("Rare NPC", "rare")},
                {1037, new Name_And_TextureName("Rare NPC", "rare")},
                {10825, new Name_And_TextureName("Rare NPC", "rare")},
                {7137, new Name_And_TextureName("Rare NPC", "rare")},
                {572, new Name_And_TextureName("Rare NPC", "rare")},
                {10824, new Name_And_TextureName("Rare NPC", "rare")},
                {2258, new Name_And_TextureName("Rare NPC", "rare")},
                {8212, new Name_And_TextureName("Rare NPC", "rare")},
                {18695, new Name_And_TextureName("Rare NPC", "rare")},
                {520, new Name_And_TextureName("Rare NPC", "rare")},
                {18697, new Name_And_TextureName("Rare NPC", "rare")},
                {18681, new Name_And_TextureName("Rare NPC", "rare")},
                {5822, new Name_And_TextureName("Rare NPC", "rare")},
                {100, new Name_And_TextureName("Rare NPC", "rare")},
                {8303, new Name_And_TextureName("Rare NPC", "rare")},
                {12433, new Name_And_TextureName("Rare NPC", "rare")},
                {10559, new Name_And_TextureName("Rare NPC", "rare")},
                {8201, new Name_And_TextureName("Rare NPC", "rare")},
                {14233, new Name_And_TextureName("Rare NPC", "rare")},
                {10078, new Name_And_TextureName("Rare NPC", "rare")},
                {61, new Name_And_TextureName("Rare NPC", "rare")},
                {11447, new Name_And_TextureName("Rare NPC", "rare")},
                {1137, new Name_And_TextureName("Rare NPC", "rare")},
                {8217, new Name_And_TextureName("Rare NPC", "rare")},
                {18690, new Name_And_TextureName("Rare NPC", "rare")},
                {10643, new Name_And_TextureName("Rare NPC", "rare")},
                {79, new Name_And_TextureName("Rare NPC", "rare")},
                {1839, new Name_And_TextureName("Rare NPC", "rare")},
                {6118, new Name_And_TextureName("Rare NPC", "rare")},
                {10356, new Name_And_TextureName("Rare NPC", "rare")},
                {1720, new Name_And_TextureName("Rare NPC", "rare")},
                {8304, new Name_And_TextureName("Rare NPC", "rare")},
                {14472, new Name_And_TextureName("Rare NPC", "rare")},
                {14429, new Name_And_TextureName("Rare NPC", "rare")},
                {6489, new Name_And_TextureName("Rare NPC", "rare")},
                {14476, new Name_And_TextureName("Rare NPC", "rare")},
                {10197, new Name_And_TextureName("Rare NPC", "rare")},
                {18693, new Name_And_TextureName("Rare NPC", "rare")},
                {14345, new Name_And_TextureName("Rare NPC", "rare")},
                {14492, new Name_And_TextureName("Rare NPC", "rare")},
                {5808, new Name_And_TextureName("Rare NPC", "rare")},
                {2605, new Name_And_TextureName("Rare NPC", "rare")},
                {16181, new Name_And_TextureName("Rare NPC", "rare")},
                {9042, new Name_And_TextureName("Rare NPC", "rare")},
                {14223, new Name_And_TextureName("Rare NPC", "rare")},
                {1936, new Name_And_TextureName("Rare NPC", "rare")},
                {1844, new Name_And_TextureName("Rare NPC", "rare")},
                {1847, new Name_And_TextureName("Rare NPC", "rare")},
                {8503, new Name_And_TextureName("Rare NPC", "rare")},
                {10899, new Name_And_TextureName("Rare NPC", "rare")},
                {99, new Name_And_TextureName("Rare NPC", "rare")},
                {10263, new Name_And_TextureName("Rare NPC", "rare")},
                {10202, new Name_And_TextureName("Rare NPC", "rare")},
                {18689, new Name_And_TextureName("Rare NPC", "rare")},
                {14231, new Name_And_TextureName("Rare NPC", "rare")},
                {14267, new Name_And_TextureName("Rare NPC", "rare")},
                {14427, new Name_And_TextureName("Rare NPC", "rare")},
                {18692, new Name_And_TextureName("Rare NPC", "rare")},
                {5935, new Name_And_TextureName("Rare NPC", "rare")},
                {2453, new Name_And_TextureName("Rare NPC", "rare")},
                {14344, new Name_And_TextureName("Rare NPC", "rare")},
                {8216, new Name_And_TextureName("Rare NPC", "rare")},
                {14488, new Name_And_TextureName("Rare NPC", "rare")},
                {8218, new Name_And_TextureName("Rare NPC", "rare")},
                {3872, new Name_And_TextureName("Rare NPC", "rare")},
                {10393, new Name_And_TextureName("Rare NPC", "rare")},
                {5826, new Name_And_TextureName("Rare NPC", "rare")},
                {3398, new Name_And_TextureName("Rare NPC", "rare")},
                {584, new Name_And_TextureName("Rare NPC", "rare")},
                {10201, new Name_And_TextureName("Rare NPC", "rare")},
                {14445, new Name_And_TextureName("Rare NPC", "rare")},
                {1848, new Name_And_TextureName("Rare NPC", "rare")},
                {5848, new Name_And_TextureName("Rare NPC", "rare")},
                {5350, new Name_And_TextureName("Rare NPC", "rare")},
                {14490, new Name_And_TextureName("Rare NPC", "rare")},
                {1838, new Name_And_TextureName("Rare NPC", "rare")},
                {14479, new Name_And_TextureName("Rare NPC", "rare")},
                {14428, new Name_And_TextureName("Rare NPC", "rare")},
                {5912, new Name_And_TextureName("Rare NPC", "rare")},
                {3586, new Name_And_TextureName("Rare NPC", "rare")},
                {5827, new Name_And_TextureName("Rare NPC", "rare")},
                {5915, new Name_And_TextureName("Rare NPC", "rare")},
                {5345, new Name_And_TextureName("Rare NPC", "rare")},
                {8279, new Name_And_TextureName("Rare NPC", "rare")},
                {472, new Name_And_TextureName("Rare NPC", "rare")},
                {14425, new Name_And_TextureName("Rare NPC", "rare")},
                {10828, new Name_And_TextureName("Rare NPC", "rare")},
                {11383, new Name_And_TextureName("Rare NPC", "rare")},
                {14478, new Name_And_TextureName("Rare NPC", "rare")},
                {8214, new Name_And_TextureName("Rare NPC", "rare")},
                {10198, new Name_And_TextureName("Rare NPC", "rare")},
                {5343, new Name_And_TextureName("Rare NPC", "rare")},
                {14268, new Name_And_TextureName("Rare NPC", "rare")},
                {763, new Name_And_TextureName("Rare NPC", "rare")},
                {2090, new Name_And_TextureName("Rare NPC", "rare")},
                {14448, new Name_And_TextureName("Rare NPC", "rare")},
                {18685, new Name_And_TextureName("Rare NPC", "rare")},
                {3470, new Name_And_TextureName("Rare NPC", "rare")},
                {2600, new Name_And_TextureName("Rare NPC", "rare")},
                {5930, new Name_And_TextureName("Rare NPC", "rare")},
                {8283, new Name_And_TextureName("Rare NPC", "rare")},
                {5831, new Name_And_TextureName("Rare NPC", "rare")},
                {1533, new Name_And_TextureName("Rare NPC", "rare")},
                {5937, new Name_And_TextureName("Rare NPC", "rare")},
                {10809, new Name_And_TextureName("Rare NPC", "rare")},
                {3773, new Name_And_TextureName("Rare NPC", "rare")},
                {3735, new Name_And_TextureName("Rare NPC", "rare")},
                {8301, new Name_And_TextureName("Rare NPC", "rare")},
                {4842, new Name_And_TextureName("Rare NPC", "rare")},
                {10642, new Name_And_TextureName("Rare NPC", "rare")},
                {14431, new Name_And_TextureName("Rare NPC", "rare")},
                {10196, new Name_And_TextureName("Rare NPC", "rare")},
                {10509, new Name_And_TextureName("Rare NPC", "rare")},
                {14236, new Name_And_TextureName("Rare NPC", "rare")},
                {1531, new Name_And_TextureName("Rare NPC", "rare")},
                {5800, new Name_And_TextureName("Rare NPC", "rare")},
                {471, new Name_And_TextureName("Rare NPC", "rare")},
                {31288, new Name_And_TextureName("Rare NPC", "rare")},
                {1140, new Name_And_TextureName("Rare NPC", "rare")},
                {14278, new Name_And_TextureName("Rare NPC", "rare")},
                {8281, new Name_And_TextureName("Rare NPC", "rare")},
                {1552, new Name_And_TextureName("Rare NPC", "rare")},
                {14266, new Name_And_TextureName("Rare NPC", "rare")},
                {5829, new Name_And_TextureName("Rare NPC", "rare")},
                {10359, new Name_And_TextureName("Rare NPC", "rare")},
                {8660, new Name_And_TextureName("Rare NPC", "rare")},
                {8199, new Name_And_TextureName("Rare NPC", "rare")},
                {5809, new Name_And_TextureName("Rare NPC", "rare")},
                {10376, new Name_And_TextureName("Rare NPC", "rare")},
                {9718, new Name_And_TextureName("Rare NPC", "rare")},
                {5933, new Name_And_TextureName("Rare NPC", "rare")},
                {4339, new Name_And_TextureName("Rare NPC", "rare")},
                {5849, new Name_And_TextureName("Rare NPC", "rare")},
                {14228, new Name_And_TextureName("Rare NPC", "rare")},
                {8979, new Name_And_TextureName("Rare NPC", "rare")},
                {1119, new Name_And_TextureName("Rare NPC", "rare")},
                {8213, new Name_And_TextureName("Rare NPC", "rare")},
                {2184, new Name_And_TextureName("Rare NPC", "rare")},
                {7016, new Name_And_TextureName("Rare NPC", "rare")},
                {14473, new Name_And_TextureName("Rare NPC", "rare")},
                {10640, new Name_And_TextureName("Rare NPC", "rare")},
                {12116, new Name_And_TextureName("Rare NPC", "rare")},
                {31284, new Name_And_TextureName("Rare NPC", "rare")},
                {6581, new Name_And_TextureName("Rare NPC", "rare")},
                {5841, new Name_And_TextureName("Rare NPC", "rare")},
                {10639, new Name_And_TextureName("Rare NPC", "rare")},
                {5916, new Name_And_TextureName("Rare NPC", "rare")},
                {2452, new Name_And_TextureName("Rare NPC", "rare")},
                {5786, new Name_And_TextureName("Rare NPC", "rare")},
                {1948, new Name_And_TextureName("Rare NPC", "rare")},
                {5837, new Name_And_TextureName("Rare NPC", "rare")},
                {16179, new Name_And_TextureName("Rare NPC", "rare")},
                {16180, new Name_And_TextureName("Rare NPC", "rare")},
                {5349, new Name_And_TextureName("Rare NPC", "rare")},
                {5796, new Name_And_TextureName("Rare NPC", "rare")},
                {14273, new Name_And_TextureName("Rare NPC", "rare")},
                {10641, new Name_And_TextureName("Rare NPC", "rare")},
                {18684, new Name_And_TextureName("Rare NPC", "rare")},
                {5793, new Name_And_TextureName("Rare NPC", "rare")},
                {616, new Name_And_TextureName("Rare NPC", "rare")},
                {25406, new Name_And_TextureName("Rare NPC", "rare")},
                {10827, new Name_And_TextureName("Rare NPC", "rare")},
                {5787, new Name_And_TextureName("Rare NPC", "rare")},
                {14341, new Name_And_TextureName("Rare NPC", "rare")},
                {5795, new Name_And_TextureName("Rare NPC", "rare")},
                {5799, new Name_And_TextureName("Rare NPC", "rare")},
                {14227, new Name_And_TextureName("Rare NPC", "rare")},
                {14226, new Name_And_TextureName("Rare NPC", "rare")},
                {2191, new Name_And_TextureName("Rare NPC", "rare")},
                {5790, new Name_And_TextureName("Rare NPC", "rare")},
                {8296, new Name_And_TextureName("Rare NPC", "rare")},
                {1910, new Name_And_TextureName("Rare NPC", "rare")},
                {4066, new Name_And_TextureName("Rare NPC", "rare")},
                {14225, new Name_And_TextureName("Rare NPC", "rare")},
                {28282, new Name_And_TextureName("Rare NPC", "rare")},
                {28280, new Name_And_TextureName("Rare NPC", "rare")},
                {32338, new Name_And_TextureName("Rare NPC", "rare")},
                {31286, new Name_And_TextureName("Rare NPC", "rare")},
                {31289, new Name_And_TextureName("Rare NPC", "rare")},
                {31071, new Name_And_TextureName("Rare NPC", "rare")},
                {39019, new Name_And_TextureName("Rare NPC", "rare")},
                {31074, new Name_And_TextureName("Rare NPC", "rare")},
                {31073, new Name_And_TextureName("Rare NPC", "rare")},
                {31072, new Name_And_TextureName("Rare NPC", "rare")},
                {31244, new Name_And_TextureName("Rare NPC", "rare")},
                {31156, new Name_And_TextureName("Rare NPC", "rare")},
                {31287, new Name_And_TextureName("Rare NPC", "rare")},
                {14342, new Name_And_TextureName("Rare NPC", "rare")},
                {26791, new Name_And_TextureName("Rare NPC", "rare")},
                {506, new Name_And_TextureName("Rare NPC", "rare")},
                {3295, new Name_And_TextureName("Rare NPC", "rare")},
                {8206, new Name_And_TextureName("Rare NPC", "rare")},
                {14275, new Name_And_TextureName("Rare NPC", "rare")},
                {14235, new Name_And_TextureName("Rare NPC", "rare")},
                {5798, new Name_And_TextureName("Rare NPC", "rare")},
                {14432, new Name_And_TextureName("Rare NPC", "rare")},
                {5832, new Name_And_TextureName("Rare NPC", "rare")},
                {16855, new Name_And_TextureName("Rare NPC", "rare")},
                {4030, new Name_And_TextureName("Rare NPC", "rare")},
                {9596, new Name_And_TextureName("Rare NPC", "rare")},
                {6228, new Name_And_TextureName("Rare NPC", "rare")},
                {10080, new Name_And_TextureName("Rare NPC", "rare")},
                {14229, new Name_And_TextureName("Rare NPC", "rare")},
                {6648, new Name_And_TextureName("Rare NPC", "rare")},
                {5834, new Name_And_TextureName("Rare NPC", "rare")},
                {10203, new Name_And_TextureName("Rare NPC", "rare")},
                {5838, new Name_And_TextureName("Rare NPC", "rare")},
                {14230, new Name_And_TextureName("Rare NPC", "rare")},
                {14346, new Name_And_TextureName("Rare NPC", "rare")},
                {15796, new Name_And_TextureName("Rare NPC", "rare")},
                {771, new Name_And_TextureName("Rare NPC", "rare")},
                {4380, new Name_And_TextureName("Rare NPC", "rare")},
                {1911, new Name_And_TextureName("Rare NPC", "rare")},
                {10817, new Name_And_TextureName("Rare NPC", "rare")},
                {3270, new Name_And_TextureName("Rare NPC", "rare")},
                {11676, new Name_And_TextureName("Rare NPC", "rare")},
                {7015, new Name_And_TextureName("Rare NPC", "rare")},
                {13977, new Name_And_TextureName("Rare NPC", "rare")},
                {14221, new Name_And_TextureName("Rare NPC", "rare")},
                {1425, new Name_And_TextureName("Rare NPC", "rare")},
                {9602, new Name_And_TextureName("Rare NPC", "rare")},
                {8282, new Name_And_TextureName("Rare NPC", "rare")},
                {11580, new Name_And_TextureName("Rare NPC", "rare")},
                {2476, new Name_And_TextureName("Rare NPC", "rare")},
                {7017, new Name_And_TextureName("Rare NPC", "rare")},
                {1424, new Name_And_TextureName("Rare NPC", "rare")},
                {6646, new Name_And_TextureName("Rare NPC", "rare")},
                {18699, new Name_And_TextureName("Rare NPC", "rare")},
                {8210, new Name_And_TextureName("Rare NPC", "rare")},
                {10357, new Name_And_TextureName("Rare NPC", "rare")},
                {14018, new Name_And_TextureName("Rare NPC", "rare")},
                {14271, new Name_And_TextureName("Rare NPC", "rare")},
                {947, new Name_And_TextureName("Rare NPC", "rare")},
                {1837, new Name_And_TextureName("Rare NPC", "rare")},
                {8280, new Name_And_TextureName("Rare NPC", "rare")},
                {5785, new Name_And_TextureName("Rare NPC", "rare")},
                {16379, new Name_And_TextureName("Rare NPC", "rare")},
                {10238, new Name_And_TextureName("Rare NPC", "rare")},
                {5864, new Name_And_TextureName("Rare NPC", "rare")},
                {14019, new Name_And_TextureName("Rare NPC", "rare")},
                {10239, new Name_And_TextureName("Rare NPC", "rare")},
                {14016, new Name_And_TextureName("Rare NPC", "rare")},
                {14506, new Name_And_TextureName("Rare NPC", "rare")},
                {8923, new Name_And_TextureName("Rare NPC", "rare")},
                {5347, new Name_And_TextureName("Rare NPC", "rare")},
                {1398, new Name_And_TextureName("Rare NPC", "rare")},
                {14279, new Name_And_TextureName("Rare NPC", "rare")},
                {11688, new Name_And_TextureName("Rare NPC", "rare")},
                {7104, new Name_And_TextureName("Rare NPC", "rare")},
                {10358, new Name_And_TextureName("Rare NPC", "rare")},
                {2192, new Name_And_TextureName("Rare NPC", "rare")},
                {1843, new Name_And_TextureName("Rare NPC", "rare")},
                {8215, new Name_And_TextureName("Rare NPC", "rare")},
                {14281, new Name_And_TextureName("Rare NPC", "rare")},
                {1112, new Name_And_TextureName("Rare NPC", "rare")},
                {1399, new Name_And_TextureName("Rare NPC", "rare")},
                {534, new Name_And_TextureName("Rare NPC", "rare")},
                {5352, new Name_And_TextureName("Rare NPC", "rare")},
                {10647, new Name_And_TextureName("Rare NPC", "rare")},
                {35074, new Name_And_TextureName("Rare NPC", "rare")},
                {8277, new Name_And_TextureName("Rare NPC", "rare")},
                {14475, new Name_And_TextureName("Rare NPC", "rare")},
                {1944, new Name_And_TextureName("Rare NPC", "rare")},
                {1841, new Name_And_TextureName("Rare NPC", "rare")},
                {14433, new Name_And_TextureName("Rare NPC", "rare")},
                {3792, new Name_And_TextureName("Rare NPC", "rare")},
                {10822, new Name_And_TextureName("Rare NPC", "rare")},
                {10082, new Name_And_TextureName("Rare NPC", "rare")},
                {8298, new Name_And_TextureName("Rare NPC", "rare")},
                {5346, new Name_And_TextureName("Rare NPC", "rare")},
                {5851, new Name_And_TextureName("Rare NPC", "rare")},
                {2186, new Name_And_TextureName("Rare NPC", "rare")},
                {8202, new Name_And_TextureName("Rare NPC", "rare")},
                {507, new Name_And_TextureName("Rare NPC", "rare")},
                {2601, new Name_And_TextureName("Rare NPC", "rare")},
                {2108, new Name_And_TextureName("Rare NPC", "rare")},
                {6651, new Name_And_TextureName("Rare NPC", "rare")},
                {6583, new Name_And_TextureName("Rare NPC", "rare")},
                {10821, new Name_And_TextureName("Rare NPC", "rare")},
                {2603, new Name_And_TextureName("Rare NPC", "rare")},
                {8203, new Name_And_TextureName("Rare NPC", "rare")},
                {6649, new Name_And_TextureName("Rare NPC", "rare")},
                {14277, new Name_And_TextureName("Rare NPC", "rare")},
                {503, new Name_And_TextureName("Rare NPC", "rare")},
                {6647, new Name_And_TextureName("Rare NPC", "rare")},
                {16184, new Name_And_TextureName("Rare NPC", "rare")},
                {31086, new Name_And_TextureName("Rare NPC", "rare")},
                {31093, new Name_And_TextureName("Rare NPC", "rare")},
                {4438, new Name_And_TextureName("Rare NPC", "rare")},
                {1885, new Name_And_TextureName("Rare NPC", "rare")},
                {8278, new Name_And_TextureName("Rare NPC", "rare")},
                {14270, new Name_And_TextureName("Rare NPC", "rare")},
                {8924, new Name_And_TextureName("Rare NPC", "rare")},
                {10236, new Name_And_TextureName("Rare NPC", "rare")},
                {9219, new Name_And_TextureName("Rare NPC", "rare")},
                {9217, new Name_And_TextureName("Rare NPC", "rare")},
                {14340, new Name_And_TextureName("Rare NPC", "rare")},
                {7895, new Name_And_TextureName("Rare NPC", "rare")},
                {25411, new Name_And_TextureName("Rare NPC", "rare")},
                {22062, new Name_And_TextureName("Rare NPC", "rare")},
                {16854, new Name_And_TextureName("Rare NPC", "rare")},
                {5836, new Name_And_TextureName("Rare NPC", "rare")},
                {5835, new Name_And_TextureName("Rare NPC", "rare")},
                {6650, new Name_And_TextureName("Rare NPC", "rare")},
                {2609, new Name_And_TextureName("Rare NPC", "rare")},
                {14487, new Name_And_TextureName("Rare NPC", "rare")},
                {5354, new Name_And_TextureName("Rare NPC", "rare")},
                {9604, new Name_And_TextureName("Rare NPC", "rare")},
                {10199, new Name_And_TextureName("Rare NPC", "rare")},
                {14426, new Name_And_TextureName("Rare NPC", "rare")},
                {5847, new Name_And_TextureName("Rare NPC", "rare")},
                {2541, new Name_And_TextureName("Rare NPC", "rare")},
                {8981, new Name_And_TextureName("Rare NPC", "rare")},
                {6652, new Name_And_TextureName("Rare NPC", "rare")},
                {14424, new Name_And_TextureName("Rare NPC", "rare")},
                {2604, new Name_And_TextureName("Rare NPC", "rare")},
                {2283, new Name_And_TextureName("Rare NPC", "rare")},
                {2602, new Name_And_TextureName("Rare NPC", "rare")},
                {14272, new Name_And_TextureName("Rare NPC", "rare")},
                {5928, new Name_And_TextureName("Rare NPC", "rare")},
                {8978, new Name_And_TextureName("Rare NPC", "rare")},
                {10237, new Name_And_TextureName("Rare NPC", "rare")},
                {14474, new Name_And_TextureName("Rare NPC", "rare")},
                {10823, new Name_And_TextureName("Rare NPC", "rare")},
                {9218, new Name_And_TextureName("Rare NPC", "rare")},
                {11467, new Name_And_TextureName("Rare NPC", "rare")},
                {10818, new Name_And_TextureName("Rare NPC", "rare")},
                {8302, new Name_And_TextureName("Rare NPC", "rare")},
                {1849, new Name_And_TextureName("Rare NPC", "rare")},
                {14446, new Name_And_TextureName("Rare NPC", "rare")},
                {14447, new Name_And_TextureName("Rare NPC", "rare")},
                {5859, new Name_And_TextureName("Rare NPC", "rare")},
                {5934, new Name_And_TextureName("Rare NPC", "rare")},
                {8976, new Name_And_TextureName("Rare NPC", "rare")},
                {1063, new Name_And_TextureName("Rare NPC", "rare")},
                {1106, new Name_And_TextureName("Rare NPC", "rare")},
                {8297, new Name_And_TextureName("Rare NPC", "rare")},
                {4015, new Name_And_TextureName("Rare NPC", "rare")},
                {1850, new Name_And_TextureName("Rare NPC", "rare")},
                {14276, new Name_And_TextureName("Rare NPC", "rare")},
                {2744, new Name_And_TextureName("Rare NPC", "rare")},
                {5932, new Name_And_TextureName("Rare NPC", "rare")},
                {5400, new Name_And_TextureName("Rare NPC", "rare")},
                {10819, new Name_And_TextureName("Rare NPC", "rare")},
                {601, new Name_And_TextureName("Rare NPC", "rare")},
                {25323, new Name_And_TextureName("Rare NPC", "rare")},
                {5348, new Name_And_TextureName("Rare NPC", "rare")},
                {10820, new Name_And_TextureName("Rare NPC", "rare")},
                {17075, new Name_And_TextureName("Rare NPC", "rare")},
                {5367, new Name_And_TextureName("Rare NPC", "rare")},
                {5789, new Name_And_TextureName("Rare NPC", "rare")},
                {9417, new Name_And_TextureName("Rare NPC", "rare")},
                {5794, new Name_And_TextureName("Rare NPC", "rare")},
                {5399, new Name_And_TextureName("Rare NPC", "rare")},
            };
        #endregion
  
        #region Ящики!
        private static IDictionary<UInt32, Name_And_TextureName> ContainerDB = new Dictionary<UInt32, Name_And_TextureName>()
            {
            {179487, new Name_And_TextureName("Затопленный сундучок",                           "container")},
            {179490, new Name_And_TextureName("Побитый сундучок",                           "container")},
            {179492, new Name_And_TextureName("Проломленный сундучок",                          "container")},
            {179488, new Name_And_TextureName("Побитый сундучок",                           "container")},
            {131978, new Name_And_TextureName("Окованный мифрилом большой сундук",          "container")},
            {179498, new Name_And_TextureName("Сундучок Алого ордена",               "container")},
            {184304, new Name_And_TextureName("Запертый сундука",               "container")},
            {20691, new Name_And_TextureName("Сундучок Коззла",               "container")},
            {191543, new Name_And_TextureName("Ящик Алого Натиска",               "container")},
            {181665, new Name_And_TextureName("Погребальный сундук",               "container")},
            {3239, new Name_And_TextureName("Сундук Бенедикта",               "container")},
            {123330, new Name_And_TextureName("Сейф буканьера",               "container")},
            {129127, new Name_And_TextureName("Сейф Галливикса",               "container")},
            {179489, new Name_And_TextureName("Затопленный сундучок",               "container")},
            {179486, new Name_And_TextureName("Побитый сундучок",               "container")},
            {123214, new Name_And_TextureName("Сундук Сумеречного леса",               "container")},
            {179494, new Name_And_TextureName("Проломленный сундучок",               "container")},
            {184793, new Name_And_TextureName("Примитивный сундук",               "container")},
            {123331, new Name_And_TextureName("Сейф буканьера",               "container")},
            {178246, new Name_And_TextureName("Учебный запертый сундук",               "container")},
            {179493, new Name_And_TextureName("Замшелый сундучок",               "container")},
            {179496, new Name_And_TextureName("Проломленный сундучок",               "container")},
            {184931, new Name_And_TextureName("Окованный оскверненным железом сундук",               "container")},
            {184934, new Name_And_TextureName("Окованный оскверненным железом сундук",               "container")},
            {105176, new Name_And_TextureName("Сейф Торговой Компании",               "container")},
            {74447, new Name_And_TextureName("Окованный железом большой сундук",               "container")},
            {153468, new Name_And_TextureName("Окованный мифрилом большой сундук",               "container")},
            {179491, new Name_And_TextureName("Затопленный сундучок",               "container")},
            {184740, new Name_And_TextureName("Плетеный ларец",               "container")},
            {184936, new Name_And_TextureName("Окованный адамантитом сундук",               "container")},
            {186648, new Name_And_TextureName("Сундучок Танзара",               "container")},
            {103815, new Name_And_TextureName("Сейф Янтарной мельницы",               "container")},
            {178244, new Name_And_TextureName("Учебный запертый сундук",               "container")},
            {121264, new Name_And_TextureName("Сейф Люция",               "container")},
            {179497, new Name_And_TextureName("Замшелый сундучок",               "container")},
            {153469, new Name_And_TextureName("Окованный мифрилом большой сундук",               "container")},
            {184741, new Name_And_TextureName("Проломленный сундучок",               "container")},
            {75295, new Name_And_TextureName("Окованный железом большой сундук",               "container")},
            {3714, new Name_And_TextureName("Сейф Альянса",               "container")},
            {123333, new Name_And_TextureName("Сейф буканьера",               "container")},
            {75297, new Name_And_TextureName("Окованный железом большой сундук",               "container")},
            {184938, new Name_And_TextureName("Окованный адамантитом сундук",               "container")},
            {184940, new Name_And_TextureName("Окованный адамантитом сундук",               "container")},
            {123332, new Name_And_TextureName("Сейф буканьера",               "container")},
            {178245, new Name_And_TextureName("Учебный запертый сундук",               "container")},
            {75296, new Name_And_TextureName("Окованный железом большой сундук",               "container")},
            {105570, new Name_And_TextureName("Сейф Альянса",               "container")},
            {184932, new Name_And_TextureName("Окованный оскверненным железом сундук",               "container")},

            };
        #endregion


        //Получить руду если она есть
        public bool GetOre(UInt32 ObjectID, ref Name_And_TextureName Ore)
        {
            return OresDB.TryGetValue(ObjectID, out Ore);
        }

        //Этот объект руда?
        public bool HasOre(UInt32 ObjectID)
        {
            Name_And_TextureName temp = new Name_And_TextureName();
            return OresDB.TryGetValue(ObjectID, out temp);
        }

        
        //Этот объект ящик хуящик?
        public bool HasContainer(UInt32 ObjectID)
        {
            Name_And_TextureName temp = new Name_And_TextureName();
            return ContainerDB.TryGetValue(ObjectID, out temp);
        }


        //Получить ящик хуящик если он есть
        public bool GetContainer(UInt32 ObjectID, ref Name_And_TextureName Container)
        {
            return ContainerDB.TryGetValue(ObjectID, out Container);
        }
        
        
        //Этот объект ящик хуящик?
        public bool HasRareNpc(UInt32 ObjectID)
        {
            Name_And_TextureName temp = new Name_And_TextureName();
            return RareNpcDB.TryGetValue(ObjectID, out temp);
        }


        //Получить ящик хуящик если он есть
        public bool GetRareNpc(UInt32 ObjectID, ref Name_And_TextureName Container)
        {
            return RareNpcDB.TryGetValue(ObjectID, out Container);
        }

        //Получить траву если она есть
        public bool GetHerb(UInt32 ObjectID, ref Name_And_TextureName Herb)
        {
            return HerbsDB.TryGetValue(ObjectID, out Herb);
        }

        //Этот объект травка?
        public bool HasHerb(UInt32 ObjectID)
        {
            Name_And_TextureName temp = new Name_And_TextureName();
            return HerbsDB.TryGetValue(ObjectID, out temp);
        }

        public bool IsResource(UInt32 objectID) => HasOre(objectID) || HasHerb(objectID);


        public bool HasInBlackList(int ObjectID)
        {
            //Всякая параша типа кораблей и т.п
            //У них прорисовка через весь континент!
            switch (ObjectID)
            {
                case 176495: return true;  //Дирижабль (Лиловая Принцесса)
                case 181689: return true;  //Дирижабль Орды ("Поцелуй небес")
                case 175080: return true;  //Дирижабль  //Ага))
                case 164871: return true;  //Дирижабль  //ыыгыы))0
                case 20808: return true;  //Корабль ("Девичий каприз")
                case 190536: return true;  //Корабль, ледокол (Гордость Штормграда)
                case 181688: return true;  //Корабль, ледокол (Северное копье)
                case 176231: return true;  //Корабль "Леди Мели"

                case 176244: return true;  //Лунная пыль
                case 177233: return true;  //Переправа Оперенной Луны
                case 186371: return true;  //Дирижабль //ЫыыЫЫыыы))
                case 187568: return true;  //Черепаха (Идущая-по-волнам)
                case 187038: return true;  //Сестренка милосердия?
                case 188511: return true;  //Черепаха (Зеленый остров)
                case 192241: return true;  //Молот Оргрима
                case 192242: return true;  //Усмиритель небес
                case 186238: return true;  //Дирижабль Орды ("Могучий ветер")
                case 181646: return true;  //Экзодарский корабль
                case 190549: return true;  //Ship (Human Test Ship)


                case 176310: return true;  //Безмятежный берег?!

                default:
                    return false;
            }
        }


    }
}

public class Name_And_TextureName
{
    public string Name { get; }
    public string TextureName { get; }

    public Name_And_TextureName(string name, string textureName)
    {
        Name = name;
        TextureName = textureName;
    }

}
