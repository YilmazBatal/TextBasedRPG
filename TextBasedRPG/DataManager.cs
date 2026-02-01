using System.Text.Json;
using TextBasedRPG.Heroes;

namespace TextBasedRPG
{
    public interface ISaveService
    {
        void SaveGame(GameContext context);
        GameContext LoadGame();
    }
    internal class DataManager : ISaveService
    {
        private readonly string _savePath = "data.json";

        #region Save
        public void SaveGame(GameContext context)
        {
            if (context.Player == null) return; // if there is no hero, it means no game progress

            // Mapping
            var saveData = new Data
            {
                IsAutoSaveOn = context.IsAutoSaveOn,

                Player = new Player
                {
                    Class = context.Player.ClassName,
                    Level = context.Player.Level,
                    Experience = context.Player.CurExp,
                    Gold = context.Player.Gold,
                    EquippedWeapon = context.Player.EquippedWeapon != null ? new ItemData
                    {
                        Name = context.Player.EquippedWeapon.Name,
                        Description = context.Player.EquippedWeapon.Description,
                        Price = context.Player.EquippedWeapon.Price,
                        Rarity = context.Player.EquippedWeapon.Rarity.ToString(),
                        ItemType = context.Player.EquippedWeapon.Type.ToString(),
                        WeaponATK = context.Player.EquippedWeapon.WeaponATK,
                        WeaponType = context.Player.EquippedWeapon.WeaponType.ToString(),
                        RequiredLevel = context.Player.EquippedWeapon.RequiredLevel
                    } : null,
                    EquippedArmor = context.Player.EquippedArmor != null ? new ItemData
                    {
                        Name = context.Player.EquippedArmor.Name,
                        Description = context.Player.EquippedArmor.Description,
                        Price = context.Player.EquippedArmor.Price,
                        Rarity = context.Player.EquippedArmor.Rarity.ToString(),
                        ItemType = context.Player.EquippedArmor.Type.ToString(),
                        ArmorDef = context.Player.EquippedArmor.ArmorDef,
                        ExtraHP = context.Player.EquippedArmor.ExtraHP,
                        RequiredLevel = context.Player.EquippedArmor.RequiredLevel
                    } : null,
                    Stats = new StatData
                    {
                        UnusedStatPoints = context.Player.UnusedStatPoints,
                        InvestedSTR = context.Player.InvestedSTRPoints,
                        InvestedVIT = context.Player.InvestedVITPoints,
                        InvestedDEX = context.Player.InvestedDEXPoints,
                        InvestedAGI = context.Player.InvestedAGIPoints,
                    }
                }
            };
            // convert items to itemdata and append to player inventory json
            List<ItemData> convertedInventory = new List<ItemData>();
            if (context.Player.Inventory != null)
            {
                foreach (var item in context.Player.Inventory)
                {
                    var itemData = new ItemData
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        Rarity = item.Rarity.ToString(),
                        ItemType = item.Type.ToString()
                    };

                    if (item is Weapon weapon)
                    {
                        itemData.WeaponATK = weapon.WeaponATK;
                        itemData.WeaponType = weapon.WeaponType.ToString();
                        itemData.RequiredLevel = weapon.RequiredLevel;
                    }
                    else if (item is Armor armor)
                    {
                        itemData.ArmorDef = armor.ArmorDef;
                        itemData.ExtraHP = armor.ExtraHP;
                        itemData.RequiredLevel = armor.RequiredLevel;
                    }
                    else if (item is Material material)
                    {
                        itemData.Quantity = material.Quantity;
                    }
                    convertedInventory.Add(itemData);
                }            
            }
               
            saveData.Player.Inventory = convertedInventory;
            
            // Convert to JSON with WriteIndented option for better readability
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(saveData, options);

            // Update the file
            File.WriteAllText(_savePath, jsonString);

            Console.WriteLine("\n[SYSTEM] Game progress saved successfully.");
            Thread.Sleep(750);
            Console.WriteLine( $"\n[SYSTEM] Auto Save is {(saveData.IsAutoSaveOn ? "ENABLED" : "DISABLED")}.");
            Thread.Sleep(1250);

        }
        #endregion
        
        #region Load
        public GameContext LoadGame()
        {
            if (!File.Exists(_savePath))
            {
                Console.WriteLine("[SYSTEM] No save file found.");
                Thread.Sleep(1000);
                Console.WriteLine("[SYSTEM] Creating a new save file...");
                Thread.Sleep(1500);

                return new GameContext();
            }

            // Read File and cache it as a string
            string jsonString = File.ReadAllText(_savePath);
            // Convert to Data object
            Data? loadedData = JsonSerializer.Deserialize<Data>(jsonString);
            // Convert to context so we can use it in the game
            var context = new GameContext();

            // Data Mapping
            if (loadedData != null)
            {
                context.IsAutoSaveOn = loadedData.IsAutoSaveOn;

                context.Player = (loadedData.Player?.Class switch
                {
                    "Warrior" => new Warrior(),
                    "Archer" => new Archer(),
                    "Mage" => new Mage(),
                    _ => new Warrior(),
                }); // %100 can't be null

                context.Player.Gold = loadedData.Player?.Gold ?? 100;
                context.Player.Level = loadedData.Player?.Level ?? 1;
                context.Player.CurExp = loadedData.Player?.Experience ?? 0;

                // load equipped items
                context.Player.EquippedWeapon = loadedData.Player?.EquippedWeapon != null ? new Weapon
                {
                    Type = Enum.TryParse<ItemType>(loadedData.Player.EquippedWeapon.ItemType, true, out var q) ? q : ItemType.Weapon,
                    Name = loadedData.Player.EquippedWeapon.Name ?? "Unknown Weapon",
                    Description = loadedData.Player.EquippedWeapon.Description ?? "No Description",
                    Price = loadedData.Player.EquippedWeapon.Price,
                    WeaponATK = loadedData.Player.EquippedWeapon.WeaponATK,
                    RequiredLevel = loadedData.Player.EquippedWeapon.RequiredLevel,
                    Rarity = Enum.TryParse<Rarity>(loadedData.Player.EquippedWeapon.Rarity, true, out var w) ? w : Rarity.Common,
                    WeaponType = Enum.TryParse<WeaponType>(loadedData.Player.EquippedWeapon.WeaponType, true, out var wept) ? wept : WeaponType.Sword
                } : null;
                context.Player.EquippedArmor = loadedData.Player?.EquippedArmor != null ? new Armor
                {
                    Type = Enum.TryParse<ItemType>(loadedData.Player.EquippedArmor.ItemType, true, out var e) ? e : ItemType.Armor,
                    Name = loadedData.Player.EquippedArmor.Name ?? "Unknown Armor",
                    Description = loadedData.Player.EquippedArmor.Description ?? "No Description",
                    Price = loadedData.Player.EquippedArmor.Price,
                    ArmorDef = loadedData.Player.EquippedArmor.ArmorDef,
                    ExtraHP = loadedData.Player.EquippedArmor.ExtraHP,
                    RequiredLevel = loadedData.Player.EquippedArmor.RequiredLevel,
                    Rarity = Enum.TryParse<Rarity>(loadedData.Player.EquippedArmor.Rarity, true, out var f) ? f : Rarity.Common,
                } : null;

                if (loadedData.Player?.Inventory != null)
                {
                    context.Player.Inventory?.Clear();
                    foreach (var item in loadedData.Player.Inventory)
                    {
                        if (item.ItemType == "Weapon")
                        {
                            context.Player.Inventory?.Add(new Weapon {
                                Type = Enum.TryParse<ItemType>(item.ItemType, true, out var t) ? t : ItemType.Weapon,
                                Name = item.Name ?? "Unknown Weapon",
                                Description = item.Description ?? "No Description",
                                Price = item.Price,
                                WeaponATK = item.WeaponATK,
                                RequiredLevel = item.RequiredLevel,
                                Rarity = Enum.TryParse<Rarity>(item.Rarity, true, out var r) ? r : Rarity.Common,
                                WeaponType = Enum.TryParse<WeaponType>(item.WeaponType, true, out var wt) ? wt : WeaponType.Sword
                            });
                        }
                        else if (item.ItemType == "Armor")
                        {
                            context.Player.Inventory?.Add(new Armor
                            {
                                Type = Enum.TryParse<ItemType>(item.ItemType, true, out var t) ? t : ItemType.Armor,
                                Name = item.Name ?? "Unknown Armor",
                                Description = item.Description ?? "No Description",
                                Price = item.Price,
                                ArmorDef = item.ArmorDef,
                                ExtraHP = item.ExtraHP,
                                RequiredLevel = item.RequiredLevel,
                                Rarity = Enum.TryParse<Rarity>(item.Rarity, true, out var r) ? r : Rarity.Common,
                            });
                        } else if (item.ItemType == "Material")
                        {
                            context.Player.Inventory?.Add(new Material
                            {
                                Type = Enum.TryParse<ItemType>(item.ItemType, true, out var t) ? t : ItemType.Material,
                                Name = item.Name ?? "Unknown Armor",
                                Description = item.Description ?? "No Description",
                                Price = item.Price,
                                Quantity = item.Quantity,
                                Rarity = Enum.TryParse<Rarity>(item.Rarity, true, out var r) ? r : Rarity.Common,
                            });
                        }
                    }

                }

                context.Player.UnusedStatPoints = loadedData.Player?.Stats?.UnusedStatPoints ?? 0;
                context.Player.InvestedSTRPoints = loadedData.Player?.Stats?.InvestedSTR ?? 0;
                context.Player.InvestedVITPoints = loadedData.Player?.Stats?.InvestedVIT ?? 0;
                context.Player.InvestedDEXPoints = loadedData.Player?.Stats?.InvestedDEX ?? 0;
                context.Player.InvestedAGIPoints = loadedData.Player?.Stats?.InvestedAGI ?? 0;
            }

            Console.WriteLine("\n[SYSTEM] Game loaded successfully.");
            Thread.Sleep(1000);
            return context;
        }
        #endregion
    }
    #region Data Models for Serialization
    public class Data
    {
        public Player? Player { get; set; }
        public bool IsAutoSaveOn { get; set; } = true;
    }
    public class Player
    {
        public string? Class { get; set; }
        public int? Level { get; set; }
        public int? Experience { get; set; }
        public int? Gold { get; set; }
        public ItemData? EquippedWeapon { get; set; }
        public ItemData? EquippedArmor { get; set; }
        public List<ItemData>? Inventory { get; set; }
        public StatData? Stats { get; set; }
    }
    /// <summary>
    /// JSON Structure for actual Item Class.
    /// We get this data from file and convert to Item class in other words context.
    /// And when saving data we convert Item class to this structure and save it to file.
    /// </summary>
    public class ItemData
    {
        public string? ItemType { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public string? Rarity { get; set; }
        public int WeaponATK { get; set; }
        public int ArmorDef { get; set; }
        public int ExtraHP { get; set; }
        public string? WeaponType { get; set; }
        public int RequiredLevel { get; set; }
        public int Quantity { get; set; }
        public int MaxStack { get; set; }
    }
    public class StatData
    {
        public int? UnusedStatPoints { get; set; }
        public int? InvestedSTR { get; set; }
        public int? InvestedVIT { get; set; }
        public int? InvestedDEX { get; set; }
        public int? InvestedAGI { get; set; }
    }
    #endregion
}
