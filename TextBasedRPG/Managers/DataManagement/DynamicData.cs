using TextBasedRPG.Heroes;
using TextBasedRPG.Models;

namespace TextBasedRPG.Managers.DataManagement
{
    public static class DynamicData
    {
        public static void LoadPlayerData(GameContext context, Data loadedData)
        {
            if (loadedData != null)
            {
                LoadGeneralData(context, loadedData);
                LoadEquippedItems(context, loadedData);
                LoadInventory(context, loadedData);
                LoadInvestedStats(context,loadedData);
            }
        }
        private static void LoadGeneralData(GameContext context,Data loadedData)
        {
            context.IsAutoSaveOn = loadedData.IsAutoSaveOn;

            context.Player = (loadedData.Player?.Class switch
            {
                "Warrior" => new Warrior(),
                "Archer" => new Archer(),
                "Mage" => new Mage(),
                _ => new Warrior(), // make it direct to the selection menu maybe?
            }); // %100 can't be null

            context.Player.Gold = loadedData.Player?.Gold ?? 100;
            context.Player.Level = loadedData.Player?.Level ?? 1;
            context.Player.CurExp = loadedData.Player?.Experience ?? 0;
            context.Player.CurHP = loadedData.Player?.CurHP ?? 1;
            context.Player.ActiveLocation = loadedData.Player?.ActiveLocation ?? "L001";
            context.Player.UnlockedUntill = loadedData.Player?.UnlockedUntill ?? 1;
        }
        private static void LoadEquippedItems(GameContext context, Data loadedData)
        {
            string? savedWeaponID = loadedData.Player?.EquippedWeapon ?? "W0001";
            string? savedArmorID = loadedData.Player?.EquippedArmor ?? "A0001";
            context.Player.EquippedWeapon = context.Weapons?.FirstOrDefault(x => x.ID == savedWeaponID);
            context.Player.EquippedArmor = context.Armors?.FirstOrDefault(x => x.ID == savedArmorID);
        }
        private static void LoadInventory(GameContext context, Data loadedData)
        {
            if (loadedData.Player?.Inventory != null)
            {
                context.Player.Inventory?.Clear();

                var allMasterItems = context.Weapons!.Cast<Item>()
                    .Concat(context.Armors!.Cast<Item>())
                    .Concat(context.Materials!.Cast<Item>())
                    .Concat(context.Consumables!.Cast<Item>())
                    .ToList();

                foreach (var itemSave in loadedData.Player.Inventory)
                {
                    var foundItem = allMasterItems.FirstOrDefault(i => i.ID == itemSave.ID);

                    if (foundItem != null)
                    {
                        InventoryData itemToAdd = new InventoryData();
                        itemToAdd.ID = itemSave.ID;
                        itemToAdd.Quantity = itemSave.Quantity;

                        context.Player.Inventory?.Add(itemToAdd);
                    }
                }
            }
        }
        private static void LoadInvestedStats(GameContext context, Data loadedData)
        {
            context.Player.UnusedStatPoints = loadedData.Player?.Stats?.UnusedStatPoints ?? 0;
            context.Player.InvestedSTRPoints = loadedData.Player?.Stats?.InvestedSTR ?? 0;
            context.Player.InvestedVITPoints = loadedData.Player?.Stats?.InvestedVIT ?? 0;
            context.Player.InvestedDEXPoints = loadedData.Player?.Stats?.InvestedDEX ?? 0;
            context.Player.InvestedAGIPoints = loadedData.Player?.Stats?.InvestedAGI ?? 0;
        }
    }
}
