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
        public void SaveGame(GameContext context)
        {
            if (context.Player == null) return; // if there is no hero, it means no game progress

            // Mapping
            var saveData = new Data
            {
                Player = new Player
                {
                    Class = context.Player.ClassName,
                    Level = context.Player.Level,
                    Experience = context.Player.Experience,
                    Gold = context.Player.Gold,
                    Stats = new StatData
                    {
                        InvestedSTR = context.Player.InvestedSTRPoints,
                        InvestedVIT = context.Player.InvestedVITPoints,
                        InvestedDEX = context.Player.InvestedDEXPoints,
                    }
                }
            };

            // Convert to JSON with WriteIndented option for better readability
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(saveData, options);

            // Update the file
            File.WriteAllText(_savePath, jsonString);

            Console.WriteLine("\n[SYSTEM] Game progress saved successfully.");
            Thread.Sleep(1000);
        }
        public GameContext LoadGame()
        {
            if (!File.Exists(_savePath))
            {
                Console.WriteLine("[SYSTEM] No save file found.");
                Thread.Sleep(1000);
                Console.WriteLine("[SYSTEM] Creating a new save file...");
                Thread.Sleep(1000);

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
                context.Player = (loadedData.Player?.Class switch
                {
                    "Warrior" => new Warrior(),
                    "Archer" => new Archer(),
                    "Mage" => new Mage(),
                    _ => new Warrior(),
                }); // %100 can't be null

                context.Player.Gold = loadedData.Player?.Gold ?? 100;
                context.Player.Level = loadedData.Player?.Level ?? 1;
                context.Player.Experience = loadedData.Player?.Experience ?? 0;
                context.Player.UnusedStatPoints = loadedData.Player?.Stats?.UnusedStatPoints ?? 0;
                context.Player.InvestedSTRPoints = loadedData.Player?.Stats?.InvestedSTR ?? 0;
                context.Player.InvestedVITPoints = loadedData.Player?.Stats?.InvestedVIT ?? 0;
                context.Player.InvestedDEXPoints = loadedData.Player?.Stats?.InvestedDEX ?? 0;
            }

            Console.WriteLine("\n[SYSTEM] Game loaded successfully.");
            Thread.Sleep(1000);
            return context;
        }
    }
    public class Data
    {
        public Player? Player { get; set; }
    }
    public class Player
    {
        public string? Class { get; set; }
        public int? Level { get; set; }
        public int? Experience { get; set; }
        public int? Gold { get; set; }
        public StatData? Stats { get; set; }
    }
    public class StatData
    {
        public int? UnusedStatPoints { get; set; }
        public int? InvestedSTR { get; set; }
        public int? InvestedVIT { get; set; }
        public int? InvestedDEX { get; set; }
    }
    public class Equipment
    {
        public string? Armor { get; set; }
        public string? Weapon { get; set; }
    }

}
