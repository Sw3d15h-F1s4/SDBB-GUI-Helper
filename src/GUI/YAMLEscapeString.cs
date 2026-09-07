
namespace GUI;

public static class YAMLEscapeString {
  public static string EscapeString(string input) {
    return input.Replace("'", "''");
  }
}
