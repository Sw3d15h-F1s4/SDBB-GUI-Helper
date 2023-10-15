using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SDBBGuiHelper.GUI
{
    internal class GuiItem
    {
        public string ItemName;
        private string DisplayName;
        private string Material;
        public int Slot;
        public List<string>? Slots;
        public List<string>? Lore;
        public int? Data;
        public int Amount;
        public string? DynamicAmount;
        public int? ModelData;
        public Dictionary<string, string>? NbtStrings;
        public Dictionary<string, int>? NbtInts;
        public Dictionary<string, string>? BannerMeta;
        public Dictionary<string, Tuple<int, int>>? PotionEffects;
        public string? EntityType;
        public Tuple<int, int, int>? RGB;
        public List<string>? ItemFlags;
        public string? BaseColor;
        public int Priority; //0 is highest priority
        public GuiRequirement ViewRequirements;
        public GuiRequirement ClickRequirements;
        public GuiRequirement? LeftClickRequirements;
        public GuiRequirement? RightClickRequirements;
        public GuiRequirement? MiddleClickRequirements;
        public GuiRequirement? ShiftLeftClickRequirements;
        public GuiRequirement? ShiftRightClickRequirements;
        public List<GuiAction> ClickCommands;
        public List<GuiAction>? LeftClickCommands;
        public List<GuiAction>? RightClickCommands;
        public List<GuiAction>? MiddleClickCommands;
        public List<GuiAction>? ShiftLeftClickCommands;
        public List<GuiAction>? ShiftRightClickCommands;
        public bool? Update;
        public Dictionary<string,int>? Enchantmnets;
        public bool? HideEnchantments;
        public bool? HideAttributes;
        public bool? HideEffects;
        public bool? HideUnbreakable;
        public bool? Unbreakable;

        public GuiItem(String item_name, String display_name, String material, int slot = 0, int amount = 1, int priority = 0)
        {
            ItemName = item_name;
            DisplayName = display_name;
            Material = material;
            Slot = slot;
            Amount = amount;
            Priority = priority;

            ViewRequirements = new();
            ClickRequirements = new();
            ClickCommands = new();
        }

        public void PrintItem(StreamWriter file, int tabLevel = 1)
        {
            file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "'", ItemName, "':"));

            tabLevel++;
            
            file.Write(IndentHandler.WriteTabbed(tabLevel, "material: "));
            file.WriteLine(Material);

            if (Data != null)
            {
                file.Write(IndentHandler.WriteTabbed(tabLevel, "data: "));
                file.WriteLine(Data);
            }
            
            if (DynamicAmount != null)
            {
                file.Write(IndentHandler.WriteTabbed(tabLevel, "dynamic_amount: "));
                file.WriteLine(DynamicAmount);
            } else
            {
                file.Write(IndentHandler.WriteTabbed(tabLevel, "amount: "));
                file.WriteLine(Amount);
            }

            if (ModelData != null)
            {
                file.Write(IndentHandler.WriteTabbed(tabLevel, "model_data: "));
                file.WriteLine(ModelData);
            }

            if (NbtStrings != null && NbtStrings?.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "nbt_strings:"));
                foreach (var str in NbtStrings) {
                    file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 1, "- '", str.Key, ":", str.Value, "'"));
                }
            }
            if (NbtInts != null && NbtInts?.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "nbt_ints:"));
                foreach (var str in NbtInts) {
                    file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 1, "- '", str.Key, ":", str.Value.ToString(), "'"));
                }
            }
            if (BannerMeta != null && BannerMeta?.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "banner_meta:"));
                foreach (var str in BannerMeta) {
                    file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 1, "- ", str.Key , ";" , str.Value));
                }
            }
            if (BaseColor != null)
            {
                file.Write(IndentHandler.WriteTabbed(tabLevel, "base_color: "));
                file.WriteLine(BaseColor);
            }
            if (ItemFlags != null && ItemFlags?.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "item_flags:"));
                foreach (var str in ItemFlags) {
                    file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 1, "- " , str));
                }
            }
            if (PotionEffects != null && PotionEffects?.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "potion_effects:"));
                foreach (var str in PotionEffects) {
                    file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 1, "- " , str.Key , ";" , str.Value.Item1.ToString() , ";" , str.Value.Item2.ToString()));
                }
            }
            if (EntityType != null)
            {
                file.Write(IndentHandler.WriteTabbed(tabLevel, "entity_type: "));
                file.WriteLine(EntityType);
            }
            if (RGB != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "rgb: ", RGB.Item1.ToString(), ", ", RGB.Item2.ToString(), ", ", RGB.Item3.ToString()));
            }


            file.Write(IndentHandler.WriteTabbed(tabLevel, "display_name: "));
            file.WriteLine("'" + DisplayName + "'");

            if (Slots == null || Slots?.Count == 0)
            {
                file.Write(IndentHandler.WriteTabbed(tabLevel, "slot: "));
                file.WriteLine(Slot.ToString());
            }
            if (Slots != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "slots: "));
                foreach (var slot in Slots)
                {
                    file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 1, "- " , slot));  
                }
            }
            if (Enchantmnets != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "enchantments: "));
                foreach (var  enchantment in Enchantmnets)
                {
                    file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 1, "- " + enchantment.Key, ";", enchantment.Value.ToString()));
                }
            }
            if (HideEnchantments != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "hide_enchantments: ", HideEnchantments.ToString().ToLower()));
            }
            if (HideAttributes != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "hide_attributes: ", HideAttributes.ToString().ToLower()));
            }
            if (HideEffects != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "hide_effects: ", HideEffects.ToString().ToLower()));
            }
            if (HideUnbreakable != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "hide_unbreakable", HideUnbreakable.ToString().ToLower()));
            }
            if (Unbreakable != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "unbreakable", Unbreakable.ToString().ToLower()));
            }

            file.Write(IndentHandler.WriteTabbed(tabLevel, "priority: "));
            file.WriteLine(Priority.ToString());

            if (Lore != null && Lore?.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "lore: "));
                foreach (var loreLine in Lore)
                {
                    file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 1, "- '" , loreLine , "'"));
                }
            }
            
            if (ViewRequirements.RequirementItems.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "view_requirement:"));
                ViewRequirements.PrintRequirements(file, tabLevel + 1);
            }

            if (ClickRequirements.RequirementItems.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "click_requirement:"));
                ClickRequirements.PrintRequirements(file, tabLevel + 1);
            }

            if (ClickCommands.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "click_commands:"));
                foreach (var clickCommand in ClickCommands)
                {
                    clickCommand.PrintAction(file, tabLevel + 1);
                }
            }
            if (LeftClickCommands != null && LeftClickCommands.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "left_click_commands:"));
                foreach (var clickCommand in LeftClickCommands)
                {
                    clickCommand.PrintAction(file, tabLevel + 1);
                }
            }
            if (RightClickCommands != null && RightClickCommands.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "right_click_commands:"));
                foreach (var clickCommand in RightClickCommands)
                {
                    clickCommand.PrintAction(file, tabLevel + 1);
                }
            }
            if (MiddleClickCommands != null && MiddleClickCommands.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "Middle_click_commands:"));
                foreach (var clickCommand in MiddleClickCommands)
                {
                    clickCommand.PrintAction(file, tabLevel + 1);
                }
            }
            if (ShiftLeftClickCommands != null && ShiftLeftClickCommands.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "shift_left_click_commands:"));
                foreach (var clickCommand in ShiftLeftClickCommands)
                {
                    clickCommand.PrintAction(file, tabLevel + 1);
                }
            }
            if (ShiftRightClickCommands != null && ShiftRightClickCommands.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "shift_right_click_commands:"));
                foreach (var clickCommand in ShiftRightClickCommands)
                {
                    clickCommand.PrintAction(file, tabLevel + 1);
                }
            }

            if (LeftClickRequirements != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "left_click_requirement:"));
                LeftClickRequirements.PrintRequirements(file, tabLevel + 1);
            }
            if (RightClickRequirements != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "right_click_requirement:"));
                RightClickRequirements.PrintRequirements(file, tabLevel + 1);
            }
            if (MiddleClickRequirements != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "middle_click_requirement:"));
                MiddleClickRequirements.PrintRequirements(file, tabLevel + 1);
            }
            if (ShiftLeftClickRequirements != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "shift_left_click_requirement:"));
                ShiftLeftClickRequirements.PrintRequirements(file, tabLevel + 1);
            }
            if (ShiftRightClickRequirements != null)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "shift_right_click_requirement:"));
                ShiftRightClickRequirements.PrintRequirements(file, tabLevel + 1);
            }
        }
    }
}
