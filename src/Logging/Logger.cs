
namespace Logging;

public sealed class Logger {

  private static readonly Lazy<Logger> _lazyInstance = new(() => new Logger());

  public static Logger Instance => _lazyInstance.Value;

  public bool VerboseMode = false;

  private Logger() { }

  public void WriteLine(string line) {
    if (VerboseMode) {
      Console.WriteLine(line);
    }
  }
}
