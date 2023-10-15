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
            redTeamCheck.AddExtraInfo("input: '%team_name%'");
            redTeamCheck.AddExtraInfo("output: 'Red'");
            GuiReqItem blueTeamCheck = new("team_check", "string equals");
            blueTeamCheck.AddExtraInfo("input: '%team_name%'");
            blueTeamCheck.AddExtraInfo("output: 'Blue'");

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
                    Menus.Add(characterName, new GuiMenu(characterName + "''s Skins!", "open" + characterName.ToLower().Replace(" ","") + "skins"));
                }
                var internalSkin = new StringBuilder();
                internalSkin.Append(characterName.ToLower().Replace(" ", "_"));
                internalSkin.Append("_");
                internalSkin.Append(skinName.ToLower().Replace(" ", "_"));
                internalSkin.Replace("&", "");

                var clickRequirements = new GuiRequirement();
                var scoreCheck = new GuiReqItem("score_check", "string equals");
                scoreCheck.AddExtraInfo("input: '%objective_score_{heroType}%'");
                scoreCheck.AddExtraInfo("output: '" + characterId + "'");
                clickRequirements.AddReqItem(scoreCheck);
                clickRequirements.AddDenyCommand("[message] &cYou need to select " + characterName + " to access this skin.");


                var viewReqRedTeam = new GuiRequirement();
                viewReqRedTeam.AddReqItem(redTeamCheck);

                var viewReqBlueTeam = new GuiRequirement();
                viewReqBlueTeam.AddReqItem(blueTeamCheck);

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


                if (permission != "(none)\r")
                {
                    var permissionCheck = new GuiReqItem("permission_check", "has permission");
                    permissionCheck.AddExtraInfo("permission: " + permission);
                    viewReqBlueTeam.AddReqItem(permissionCheck);
                    viewReqRedTeam.AddReqItem(permissionCheck);
                }

                var newSkinRed = new GuiItem(internalSkin.ToString()+"_red", skinName, skinHeadRed, slotNumber, 1);

                newSkinRed.AddLore(skinRarity);
                newSkinRed.AddViewRequirement(viewReqRedTeam);
                newSkinRed.AddClickRequirement(clickRequirements);
                newSkinRed.AddClickCommand("[player] skin url " + skinRedLink);
                newSkinRed.AddClickCommand("[close]");
                
                
                var newSkinBlue = new GuiItem(internalSkin.ToString()+"_blue", skinName, skinHeadBlue, slotNumber, 2);

                newSkinBlue.AddLore(skinRarity);
                newSkinBlue.AddViewRequirement(viewReqBlueTeam);
                newSkinBlue.AddClickRequirement(clickRequirements);
                newSkinBlue.AddClickCommand("[player] skin url " + skinBlueLink);
                newSkinBlue.AddClickCommand("[close]");


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
                GuiItem goBack = new("goback", "Go Back", "BARRIER", menu.Value.InventorySize - 1);
                goBack.AddClickCommand("[openguimenu] skins_menu");
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
                link.AddClickCommand("[openguimenu] " + item.Key.ToLower().Replace(" ", "_") + "_menu");
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
