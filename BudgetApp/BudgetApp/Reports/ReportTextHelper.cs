using System.Globalization;

namespace Budget_App.Reports {
  internal static class ReportTextHelper {
    public static string FormatMoney(double amount) {
      string text = amount.ToString("0.00", CultureInfo.InvariantCulture);
      return text;
    }
  }
}
