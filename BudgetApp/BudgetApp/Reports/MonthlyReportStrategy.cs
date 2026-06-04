using System;
using System.Text;
using Budget_App.Models;

namespace Budget_App.Reports {
  internal class MonthlyReportStrategy : IReportStrategy {
    public string Generate(ReportData data) {
      if (data == null || data.Budget == null) {
        return "Нет данных бюджета для отчёта.";
      }

      StringBuilder builder = new StringBuilder();
      builder.AppendLine("========== ОТЧЁТ ЗА ПЕРИОД ==========");
      builder.AppendLine("Бюджет: " + data.Budget.Name);
      builder.AppendLine(
        "Период: "
        + data.Budget.PeriodStart.ToString("yyyy-MM-dd")
        + " .. "
        + data.Budget.PeriodEnd.ToString("yyyy-MM-dd"));
      builder.AppendLine("------------------------------------");
      builder.AppendLine("Дата       | Категория | Сумма    | Описание");
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
        string amountText = ReportTextHelper.FormatMoney(expense.Amount);
        builder.AppendLine(
          expense.Date.ToString("yyyy-MM-dd")
          + " | "
          + expense.CategoryName
          + " | "
          + amountText
          + " | "
          + expense.Description);
        periodTotal = periodTotal + expense.Amount;
      }

      builder.AppendLine("------------------------------------");
      string totalText = ReportTextHelper.FormatMoney(periodTotal);
      builder.AppendLine("Итого за период: " + totalText);
      builder.AppendLine("====================================");
      return builder.ToString();
    }
  }
}
