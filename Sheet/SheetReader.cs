using SDBBGuiHelper.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SDBBGuiHelper.Sheet
{
    internal class SheetReader
    {

        private StreamReader Sheet;

        private Dictionary<string, GuiMenu> Menus;

        private Dictionary<string, string> Defaults;

        public SheetReader(StreamReader file)
        {
            Menus = new();
            Defaults = new(); 
            Sheet = file;  
        }

        public void ReadCharSkinSheet()
        {
            var lines = Sheet.ReadToEnd().Split(new char[] { '\n' });

            GuiReqItem redTeamCheck = new("team_check", "string equals");
            redTeamCheck.Extra.Add("input: '%team_name%'");
            redTeamCheck.Extra.Add("output: 'Red'");
            GuiReqItem blueTeamCheck = new("team_check", "string equals");
            blueTeamCheck.Extra.Add("input: '%team_name%'");
            blueTeamCheck.Extra.Add("output: 'Blue'");

            foreach (var row in lines)
            {
                if (row.Length == 0)
                {
                    Console.WriteLine("[WARN] Skipping a line, nothing found.");
                    continue;
                }
                var cols = row.Split(new char[] { '\t' });

                if (cols.Length != 10)
                {
                    // throw new Exception("Improperly formatted spreadhseet.");
                    Console.WriteLine("[ERROR] Improperly formatted spreadsheet line. LINE:" + row);
                    continue;
                }

                if (cols[0] == "" || cols[0].ToLower() == "character")
                {
                    Console.WriteLine("[WARN] Skipping a line, header or just empty.");
                    continue;
                }

                var characterName = cols[0];
                var characterId = cols[1];
                var skinName = cols[2];
                var skinRarity = cols[3];
                var skinBlueLink = cols[4];
                var skinRedLink = cols[5];
                var skinHeadBlue = cols[6];
                var skinHeadRed = cols[7];
                var slotNumber = int.Parse(cols[8]);
                var permission = cols[9];

                if (!Menus.ContainsKey(characterName))
                {
                    Menus.Add(characterName, new GuiMenu(characterName + "''s Skins!", "open" + characterName.ToLower().Replace(" ", "") + "skins"));
                }
                var internalSkin = new StringBuilder();
                internalSkin.Append(characterName.ToLower().Replace(' ', '_'));
                internalSkin.Append('_');
                internalSkin.Append(skinName.ToLower().Replace(' ', '_'));
                internalSkin.Replace("&", "");

                var clickRequirements = new GuiRequirement()
                {
                    RequirementItems =
                    {
                        new("score_check", "string equals")
                        {
                            Extra =
                            {
                                "input: '%objective_score_{heroType}%'",
                                "output: '" + characterId + "'",
                            }
                        }
                    },
                    DenyCommands =
                    {
                        new(ActionTypes.Message, " &cYou need to select ", characterName, " to access this skin.")
                    }
                    
                };

                var viewReqRedTeam = new GuiRequirement()
                {
                    RequirementItems = { redTeamCheck }
                };

                var viewReqBlueTeam = new GuiRequirement()
                {
                    RequirementItems = { blueTeamCheck }
                };

                switch (skinRarity)
                {
                    case "Common":
                        skinName = "&r&a&l" + skinName;
                        break;
                    case "Rare":
                        skinName = "&r&b&l" + skinName;
                        break;
                    case "Epic":
                        skinName = "&r&d&l" + skinName;
                        break;
                    case "Legendary":
                        skinName = "&r&6&l" + skinName;
                        break;
                    case "Mythical":
                        skinName = "&r&4&kABC &r&4&l" + skinName + " &r&4&kABC";
                        break;
                    case "Legacy":
                        skinName = "&r&f" + skinName;
                        break;
                    default:
                        skinName = "&r&e" + skinName;
                        break;
                }


                if (!permission.Contains("none"))
                {
                    var permissionCheck = new GuiReqItem("permission_check", "has permission");
                    permission = permission.Replace("\r", "");
                    permissionCheck.Extra.Add("permission: " + permission);
                    viewReqBlueTeam.RequirementItems.Add(permissionCheck);
                    viewReqRedTeam.RequirementItems.Add(permissionCheck);
                }

                var newSkinRed = new GuiItem(internalSkin.ToString() + "_red", skinName, skinHeadRed)
                {
                    Slot = slotNumber,
                    Lore = new() { skinRarity },
                    Priority = 1,
                    ViewRequirements = viewReqRedTeam,
                    ClickRequirements = clickRequirements,
                    ClickCommands =
                    {
                        new(ActionTypes.Player, " skin url ", skinRedLink),
                        new(ActionTypes.Close),
                    }
                };
                var newSkinBlue = new GuiItem(internalSkin.ToString() + "_red", skinName, skinHeadBlue)
                {
                    Slot = slotNumber,
                    Lore = new() { skinRarity },
                    Priority = 2,
                    ViewRequirements = viewReqBlueTeam,
                    ClickRequirements = clickRequirements,
                    ClickCommands =
                    {
                        new(ActionTypes.Player, " skin url ", skinBlueLink),
                        new(ActionTypes.Close),
                    }
                };

                Menus[characterName].AddItem(newSkinRed);
                Menus[characterName].AddItem(newSkinBlue);


                if (skinRarity == "Default Skin")
                {
                    Defaults.Add(characterName, skinHeadRed);
                }
            }
            Sheet.Close();

            foreach(var menu in Menus)
            {
                GuiItem goBack = new("goback", "Go Back", "BARRIER", ((int)menu.Value.InventorySize) - 1);
                goBack.ClickCommands.Add(new(ActionTypes.OpenGuiMenu, " skins_menu"));
                menu.Value.AddItem(goBack);
            }

            MakeMainSkinMenu();
        }

        private void MakeMainSkinMenu()
        {
            GuiMenu skinsMenu = new("Skins Menu", "skinsmenu", true);
            var slot = 0;
            foreach (var item in Defaults)
            {
                GuiItem link = new(item.Key.ToLower().Replace(" ", ""), item.Key + " Skins...", item.Value, slot);
                slot++;
                link.ClickCommands.Add(new(ActionTypes.OpenGuiMenu, " ", item.Key.ToLower().Replace(" ", "_") , "_menu"));
                skinsMenu.AddItem(link);
            }
            Menus.Add("skins", skinsMenu);
        }

        public void PrintMenus(string outputDir)
        {
            Directory.CreateDirectory(outputDir);
            foreach (var item in Menus)
            {
                item.Value.PrintMenu(new(Path.Combine(outputDir, item.Key.ToLower().Replace(" ","_") + "_menu.yml")));
            }
        }
        public void PrintMenuConfig(string outputDir)
        {
            Directory.CreateDirectory(outputDir);
            var file = new StreamWriter(Path.Combine(outputDir, "config_EXAMPLE.yml"));
            
            foreach (var item in Menus)
            {
                file.WriteLine("  " + item.Key.ToLower().Replace(" ", "_") + "_menu:");
                file.WriteLine("    file: skins/" + item.Key.ToLower().Replace(" ", "_") + "_menu.yml");
            }
            file.Close();
        }

    }
}
