
namespace Reader;

public class CharacterSkin(string SkinName,
                              string Rarity,
                              string BlueTeamLink,
                              string RedTeamLink,
                              string BlueTeamHead,
                              string RedTeamHead,
                              string ViewPermission)
                           : IComparable<CharacterSkin>
{
  public string SkinName { get; set; }  = SkinName;
  public string Rarity { get; }         = Rarity;
  public string BlueTeamLink { get; }   = BlueTeamLink;
  public string RedTeamLink { get; }    = RedTeamLink;
  public string BlueTeamHead { get; }   = BlueTeamHead;
  public string RedTeamHead { get; }    = RedTeamHead;
  public string ViewPermission { get; set;} = ViewPermission;
  public int SlotNumber = 0;

  public int CompareTo(CharacterSkin? otherSkin) {
    if (otherSkin == null) {
      return 1;
    }

    var thisRarNum = RarityToNumber(Rarity);
    var thatRarNum = RarityToNumber(otherSkin.Rarity);

    if (thisRarNum > thatRarNum) {
      //this is more rare
      return 1;
    } else if (thisRarNum < thatRarNum) {
      //this is less rare
      return -1;
    } else {
      //sort alphabetically within a rarity tier
      return SkinName.CompareTo(otherSkin.SkinName);
    }
  }

  static private int RarityToNumber(string rarity) {
    return rarity switch {
      "Default Skin" => 1,
      "Legacy"       => 2,
      "Mythical"     => 3,
      "Legendary"    => 4,
      "Epic"         => 5,
      "Rare"         => 6,
      "Common"       => 7,
      _              => 8,
    };
  }

}

struct Character(string CharcterName, string CharacterID)
{
    public string CharacterName { get; } = CharcterName;
    public string CharacterID { get; }  = CharacterID;
    public List<CharacterSkin> SkinList = [];
}
