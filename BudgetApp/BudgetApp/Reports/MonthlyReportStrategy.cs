using System;
using System.Text;
using Budget_App.Models;

namespace Budget_App.Reports {
  /// <summary>
  /// Report with expenses inside the budget period.
  /// </summary>
  internal class MonthlyReportStrategy : IReportStrategy {
    /// <inheritdoc />
    public string Generate(ReportData data) {
      if (data == null || data.Budget == null) {
        return "No budget data for report.";
      }

      StringBuilder builder = new StringBuilder();
      builder.AppendLine("========== MONTHLY REPORT ==========");
      builder.AppendLine("Budget: " + data.Budget.Name);
      builder.AppendLine(
        "Period: "
        + data.Budget.PeriodStart.ToString("yyyy-MM-dd")
        + " .. "
        + data.Budget.PeriodEnd.ToString("yyyy-MM-dd"));
      builder.AppendLine("------------------------------------");

      double periodTotal = 0.0;
      int expenseCount = data.ExpenseList.Count;
      for (int expenseIndex = 0; expenseIndex < expenseCount; expenseIndex++) {
        Expense expense = data.ExpenseList[expenseIndex];
        if (expense.Date < data.Budget.PeriodStart) {
          continue;
        }
        if (expense.Date > data.Budget.PeriodEnd) {
          continue;
        }
        builder.AppendLine(
          expense.Date.ToString("yyyy-MM-dd")
          + " | "
          + expense.CategoryName
          + " | "
          + expense.Amount.ToString("0.00")
          + " | "
          + expense.Description);
        periodTotal = periodTotal + expense.Amount;
      }

      builder.AppendLine("------------------------------------");
      builder.AppendLine("Total for period: " + periodTotal.ToString("0.00"));
      builder.AppendLine("====================================");
      return builder.ToString();
    }
  }
}
