using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBBGuiHelper.GUI
{
    struct InventoryTypes
    {
        public const string Chest = "CHEST";
        public const string Anvil = "ANVIL";
        public const string Barrel = "BARREL";
        public const string Beacon = "BEACON";
        public const string BlastFurnace = "BLAST_FURNACE";
        public const string BrewingStand = "BREWING";
        public const string CartographyTable = "CARTOGRAPHY";
        public const string Dispenser = "DISPENSER";
        public const string Dropper = "DROPPER";
        public const string EnchantingTable = "ENCHANTING";
        public const string EnderChest = "ENDER_CHEST";
        public const string Furnace = "FURNACE";
        public const string Grindstone = "GRINDSTONE";
        public const string Hopper = "HOPPER";
        public const string Loom = "LOOM";
        public const string PlayerInventory = "PLAYER";
        public const string ShulkerBox = "SHULKER_BOX";
        public const string Smoker = "SMOKER";
        public const string Workbench = "WORKBENCH";
    }

    class ActionTypes
    {
        public const string Player = "[player]";
        public const string Console = "[console]";
        public const string CommandEvent = "[commandevent]";
        public const string Placeholder = "[placeholder]";
        public const string Message = "[message]";
        public const string Broadcast = "[broadcast]";
        public const string MiniMessage = "[minimessage]";
        public const string MiniBroadcast = "[minibroadcast]";
        public const string OpenGuiMenu = "[openguimenu]";
        public const string Connect = "[connect]";
        public const string Close = "[close]";
        public const string JSONMessage = "[json]";
        public const string JSONBroadcast = "[jsonbroadcast]";
        public const string Refresh = "[refresh]";
        public const string BroadcastSound = "[broadcastsound]";
        public const string BroadcastSoundWorld = "[broadcastsoundworld]";
        public const string Sound = "[sound]";
        public const string TakeMoney = "[takemoney]";
        public const string GiveMoney = "[givemoney]";
        public const string TakeXP = "[takeexp]";
        public const string GiveXP = "[givexp]";
        public const string GivePermission = "[givepermission]";
        public const string TakePermission = "[takepermission]";
        public const string Meta = "[meta]";
        public const string Chat = "[chat]";
    }
}
