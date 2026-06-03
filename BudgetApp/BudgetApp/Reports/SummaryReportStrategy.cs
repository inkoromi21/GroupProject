using System.Text;
using Budget_App.Models;

namespace Budget_App.Reports {
  internal class SummaryReportStrategy : IReportStrategy {
    public string Generate(ReportData data) {
      if (data == null || data.Budget == null) {
        return "Нет данных бюджета для отчёта.";
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
      builder.AppendLine("========== СВОДНЫЙ ОТЧЁТ ==========");
      builder.AppendLine("Бюджет: " + data.Budget.Name);
      builder.AppendLine("Лимит: " + ReportTextHelper.FormatMoney(limitAmount));
      builder.AppendLine("Потрачено: " + ReportTextHelper.FormatMoney(spentTotal));
      builder.AppendLine("Остаток: " + ReportTextHelper.FormatMoney(balanceAmount));
      builder.AppendLine("==================================");
      return builder.ToString();
    }
  }
}
