using System.Globalization;
using System.Text;
using Budget_App.Models;

namespace Budget_App.Reports {
  /// <summary>
  /// Short summary: limit, spent, balance.
  /// </summary>
  internal class SummaryReportStrategy : IReportStrategy {
    /// <inheritdoc />
    public string Generate(ReportData data) {
      if (data == null || data.Budget == null) {
        return "No budget data for report.";
      }

      double spentTotal = 0.0;
      int expenseCount = data.ExpenseList.Count;
      for (int expenseIndex = 0; expenseIndex < expenseCount; expenseIndex++) {
        Expense expense = data.ExpenseList[expenseIndex];
        spentTotal = spentTotal + expense.Amount;
      }

      double limitAmount = data.Budget.TotalLimit;
      double balanceAmount = limitAmount - spentTotal;

      StringBuilder builder = new StringBuilder();
      builder.AppendLine("========== SUMMARY REPORT ==========");
      builder.AppendLine("Budget: " + data.Budget.Name);
      builder.AppendLine("Limit: " + limitAmount.ToString("0.00", CultureInfo.InvariantCulture));
      builder.AppendLine("Spent: " + spentTotal.ToString("0.00", CultureInfo.InvariantCulture));
      builder.AppendLine("Balance: " + balanceAmount.ToString("0.00", CultureInfo.InvariantCulture));
      builder.AppendLine("==================================");
      return builder.ToString();
    }
  }
}
