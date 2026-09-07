using System.Text;
using GUI;

namespace Readers;

internal class SheetReader(StreamReader file)
{
    private readonly GuiMenu Menu = new("Skins Menu", "skinsmenu", true);

    public void ReadCharSkinSheet()
    {
        var lines = file.ReadToEnd().Split(['\n']);

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
            var cols = row.Split(['\t']);

            if (cols.Length != 10)
            {
                // throw new Exception("Improperly formatted spreadhseet.");
                Console.WriteLine("[ERROR] Improperly formatted spreadsheet line. LINE:" + row);
                continue;
            }

            if (
                cols[0] == ""
                || string.Equals(cols[0], "character", StringComparison.OrdinalIgnoreCase)
            )
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

            GuiReqItem characterCheck = new("score_check", "string equals");
            characterCheck.Extra.Add("input: '%objective_score_{heroType}%'");
            characterCheck.Extra.Add("output: '" + characterId + "'");

            var internalSkin = new StringBuilder();
            internalSkin.Append(characterName.ToLower().Replace(' ', '_'));
            internalSkin.Append('_');
            internalSkin.Append(skinName.ToLower().Replace(' ', '_'));
            internalSkin.Replace("&", "");

            var clickRequirements = new GuiRequirement()
            {
                RequirementItems = { characterCheck },

                DenyCommands =
                {
                    new(
                        ActionTypes.Message,
                        " &cYou need to select ",
                        characterName,
                        " to access this skin."
                    ),
                },
            };

            var viewReqRedTeam = new GuiRequirement()
            {
                RequirementItems = { redTeamCheck, characterCheck },
            };

            var viewReqBlueTeam = new GuiRequirement()
            {
                RequirementItems = { blueTeamCheck, characterCheck },
            };

            skinName = skinRarity switch
            {
                "Common" => "&r&a&l" + skinName,
                "Rare" => "&r&b&l" + skinName,
                "Epic" => "&r&d&l" + skinName,
                "Legendary" => "&r&6&l" + skinName,
                "Mythical" => "&r&4&kABC &r&4&l" + skinName + " &r&4&kABC",
                "Legacy" => "&r&f" + skinName,
                _ => "&r&e" + skinName,
            };

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
                Lore = [skinRarity],
                Priority = 1 + int.Parse(characterId),
                ViewRequirements = viewReqRedTeam,
                ClickRequirements = clickRequirements,
                ClickCommands =
                {
                    new(ActionTypes.Player, " skin url ", skinRedLink),
                    new(ActionTypes.Close),
                },
            };
            var newSkinBlue = new GuiItem(internalSkin.ToString() + "_blue", skinName, skinHeadBlue)
            {
                Slot = slotNumber,
                Lore = [skinRarity],
                Priority = 100 + int.Parse(characterId),
                ViewRequirements = viewReqBlueTeam,
                ClickRequirements = clickRequirements,
                ClickCommands =
                {
                    new(ActionTypes.Player, " skin url ", skinBlueLink),
                    new(ActionTypes.Close),
                },
            };

            Menu.AddItem(newSkinRed);
            Menu.AddItem(newSkinBlue);
            Menu.InventorySize = GuiMenu.InventorySizes.TWO_ROWS;
        }
        file.Close();
    }

    public void PrintMenu(string outputDir)
    {
        Directory.CreateDirectory(outputDir);
        Menu.PrintMenu(new(Path.Combine(outputDir, "skinsmenu.yml")));
    }
}
