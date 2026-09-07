using System.Text;
using GUI;
using Logging;

namespace Reader;

internal class SheetReader(StreamReader file)
{
    private readonly GuiMenu Menu = new("Skins Menu", "skinsmenu", true);

    private readonly Dictionary<string, Character> Characters = [];

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
                Logger.Instance.WriteLine("WRN: Row " + row + " is empty.");
                continue;
            }
            var cols = row.Split(['\t']);

            if (cols.Length != 9)
            {
                Logger.Instance.WriteLine("WRN: Wrong number of colums on line " + row);
                continue;
            }

            if (
                cols[0] == ""
                || string.Equals(cols[0], "character", StringComparison.OrdinalIgnoreCase)
            )
            {
                Logger.Instance.WriteLine("WRN: Skipping a line, this could be a header");
                continue;
            }

            var characterName = cols[0];
            var characterId   = cols[1];
            var skinName      = cols[2];
            var skinRarity    = cols[3];
            var skinBlueLink  = cols[4];
            var skinRedLink   = cols[5];
            var skinHeadBlue  = cols[6];
            var skinHeadRed   = cols[7];
            var permission    = cols[8];

            CharacterSkin skinObj = new(
              skinName,
              skinRarity,
              skinBlueLink,
              skinRedLink,
              skinHeadBlue,
              skinHeadRed,
              permission
            );

            if (Characters.TryGetValue(characterName, out Character value)) {
                value.SkinList.Add(skinObj);
                Logger.Instance.WriteLine("INF: Added skin: " + skinName);
            } else {
              Characters.Add(characterName, new(characterName, characterId));
              Characters[characterName].SkinList.Add(skinObj);
              Logger.Instance.WriteLine("INF: Discovered character: " + characterName);
              Logger.Instance.WriteLine("INF: Added skin: " + skinName);
            }

        }


        int largestSlot = 0;

        foreach (var keyValue in Characters) {
          var character = keyValue.Value;

          character.SkinList.Sort();
          Logger.Instance.WriteLine("INF: Sorted list for " + character.CharacterName + " successfully.");

          int slotNumber = 0;

          foreach (var skin in character.SkinList) {

            GuiReqItem characterCheck = new("score_check", "string equals");
            characterCheck.Extra.Add("input: '%objective_score_{heroType}%'");
            characterCheck.Extra.Add("output: '" + character.CharacterID + "'");

            var internalSkin = new StringBuilder();
            internalSkin.Append(character.CharacterName.ToLower().Replace(' ', '_'));
            internalSkin.Append('_');
            internalSkin.Append(skin.SkinName.ToLower().Replace(' ', '_'));
            internalSkin.Replace("&", "");

            var clickRequirements = new GuiRequirement()
            {
                RequirementItems = { characterCheck },

                DenyCommands =
                {
                    new(
                        ActionTypes.Message,
                        " &cYou need to select ",
                        character.CharacterName,
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

            skin.SkinName = skin.Rarity switch
            {
                "Common" => "&r&a&l" + skin.SkinName,
                "Rare" => "&r&b&l" + skin.SkinName,
                "Epic" => "&r&d&l" + skin.SkinName,
                "Legendary" => "&r&6&l" + skin.SkinName,
                "Mythical" => "&r&4&kABC &r&4&l" + skin.SkinName + " &r&4&kABC",
                "Legacy" => "&r&f" + skin.SkinName,
                _ => "&r&e" + skin.SkinName,
            };

            if (!skin.ViewPermission.Contains("none"))
            {
                var permissionCheck = new GuiReqItem("permission_check", "has permission");
                skin.ViewPermission = skin.ViewPermission.Replace("\r", "");
                permissionCheck.Extra.Add("permission: " + skin.ViewPermission);
                viewReqBlueTeam.RequirementItems.Add(permissionCheck);
                viewReqRedTeam.RequirementItems.Add(permissionCheck);
            }

            var newSkinRed = new GuiItem(internalSkin.ToString() + "_red", skin.SkinName, skin.RedTeamHead)
            {
                Slot = slotNumber,
                Lore = [skin.Rarity],
                Priority = 1 + int.Parse(character.CharacterID),
                ViewRequirements = viewReqRedTeam,
                ClickRequirements = clickRequirements,
                ClickCommands =
                {
                    new(ActionTypes.Player, " skin url ", skin.RedTeamLink),
                    new(ActionTypes.Close),
                },
            };
            var newSkinBlue = new GuiItem(internalSkin.ToString() + "_blue", skin.SkinName, skin.BlueTeamHead)
            {
                Slot = slotNumber,
                Lore = [skin.Rarity],
                Priority = 1000 + int.Parse(character.CharacterID),
                ViewRequirements = viewReqBlueTeam,
                ClickRequirements = clickRequirements,
                ClickCommands =
                {
                    new(ActionTypes.Player, " skin url ", skin.BlueTeamLink),
                    new(ActionTypes.Close),
                },
            };

            Menu.AddItem(newSkinRed);
            Menu.AddItem(newSkinBlue);

            switch (slotNumber) {
              case 0:
                slotNumber = 2;
                break;
              case 8:
                slotNumber = 11;
                break;
              case 17:
                slotNumber = 20;
                break;
              case 26:
                slotNumber = 29;
                break;
              case 35:
                slotNumber = 38;
                break;
              case 44:
                slotNumber = 47;
                break;
              case 54:
                Console.Error.WriteLine("ERR: Can't fit all the skins! We are doomed!");
                slotNumber = 1;
                break;
              default:
                slotNumber++;
                break;
            };
          }

          largestSlot = int.Max(largestSlot, slotNumber);
        }

        Logger.Instance.WriteLine("INF: Highest slot number is " + largestSlot);
        file.Close();
    }

    public void PrintMenu(string outputDir)
    {
        Directory.CreateDirectory(outputDir);
        Menu.PrintMenu(new(Path.Combine(outputDir, "skinsmenu.yml")));
    }
}
