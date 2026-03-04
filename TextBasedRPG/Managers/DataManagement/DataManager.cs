using System.Text.Json;
using System.Text.Json.Serialization;
using TextBasedRPG.Events;
using TextBasedRPG.Items;
using TextBasedRPG.Locations;
using TextBasedRPG.Models;
using TextBasedRPG.UI;

namespace TextBasedRPG.Managers.DataManagement
{
    internal class DataManager : ISaveService
    {
        private readonly string _savePath = "data.json";

        #region Save
        public void SaveGame(GameContext context)
        {
            if (context == null) return; // if there is no hero, it means no game progress

            // Mapping
            var saveData = new Data
            {
                IsAutoSaveOn = context.IsAutoSaveOn,

                Player = new Player
                {
                    Class = context.Player.ClassName,
                    Level = context.Player.Level,
                    ActiveLocation = context.Player.ActiveLocation,
                    UnlockedUntill = context.Player.UnlockedUntill,
                    Experience = context.Player.CurExp,
                    CurHP = context.Player.CurHP,
                    Gold = context.Player.Gold,
                    EquippedWeapon = context.Player.EquippedWeapon?.ID,
                    EquippedArmor = context.Player.EquippedArmor?.ID,

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
            List<InventoryData> convertedInventory = new List<InventoryData>();

            if (context.Player.Inventory != null)
            {
                foreach (var item in context.Player.Inventory)
                {
                    var itemData = new InventoryData
                    {
                        ID = item.ID,
                        Quantity = item.Quantity
                    };
                    convertedInventory.Add(itemData);
                }
            }

            saveData.Player.Inventory = convertedInventory;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(saveData, options);

            // Update the file
            File.WriteAllText(_savePath, jsonString);

            MenuUI.ColoredMsg(ConsoleColor.Green, "\n[SYSTEM] Game progress saved successfully.");
            Thread.Sleep(400);
            MenuUI.ColoredMsg(ConsoleColor.Yellow, $"\n[SYSTEM] Auto Save is {(saveData.IsAutoSaveOn ? "ENABLED" : "DISABLED")}.");
            Thread.Sleep(400);

        }
        #endregion

        #region Load
        public GameContext LoadGame()
        {
            if (!File.Exists(_savePath))
            {
                MenuUI.ColoredMsg(ConsoleColor.Red, "[SYSTEM] No save file found.");
                Thread.Sleep(400);
                MenuUI.ColoredMsg(ConsoleColor.Yellow, "[SYSTEM] Creating a new save file...");
                Thread.Sleep(400);

                var newContext = new GameContext();

                StaticData.LoadStaticDatas(newContext);

                newContext.Player = null;

                InitializeEvents(newContext);

                return newContext;
            }

            // Read File and cache it as a string
            string jsonString = File.ReadAllText(_savePath);
            // Convert to Data object
            Data? loadedData = JsonSerializer.Deserialize<Data>(jsonString);
            // Convert to context so we can use it in the game
            var context = new GameContext();

            // Load Database to Cache
            StaticData.LoadStaticDatas(context);

            // Data Mapping
            DynamicData.LoadPlayerData(context, loadedData!);

            InitializeEvents(context);

            MenuUI.ColoredMsg(ConsoleColor.Green, "\n[SYSTEM] Game loaded successfully.");
            Thread.Sleep(1000);
            return context;
        }
        #endregion


        /// <summary>
        /// Subscribe to events
        /// </summary>
        /// <param name="context"></param>
        private static void InitializeEvents(GameContext context)
        {
            // clearing in case cuz defensive programming
            EventManager.HeroEvents.OnExpGained -= (amount) => LevelManager.CheckLevelUp(context);
            EventManager.HeroEvents.OnExpGained += (amount) => LevelManager.CheckLevelUp(context);
        }

    }
    public class Data
    {
        public Player? Player { get; set; }
        public bool IsAutoSaveOn { get; set; } = true;
        [JsonIgnore]
        public List<MobData>? EntityList { get; set; }
        [JsonIgnore]
        public List<Location>? Locations { get; set; }
        [JsonIgnore]
        public List<Weapon>? Weapons { get; set; }
        [JsonIgnore]
        public List<Armor>? Armors { get; set; }
        [JsonIgnore]
        public List<Material>? Materials { get; set; }
        [JsonIgnore]
        public List<Consumable>? Consumables { get; set; }
    }
}
