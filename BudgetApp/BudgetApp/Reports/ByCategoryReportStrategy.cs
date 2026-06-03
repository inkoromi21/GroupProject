using System.Collections.Generic;
using System.Text;
using Budget_App.Models;

namespace Budget_App.Reports {
  /// <summary>
  /// Report with totals grouped by expense category.
  /// </summary>
  internal class ByCategoryReportStrategy : IReportStrategy {
    /// <inheritdoc />
    public string Generate(ReportData data) {
      if (data == null || data.Budget == null) {
        return "No budget data for report.";
      }

      StringBuilder builder = new StringBuilder();
      builder.AppendLine("========== REPORT BY CATEGORY ==========");
      builder.AppendLine("Budget: " + data.Budget.Name);
      builder.AppendLine("----------------------------------------");

      List<string> categoryNameList = new List<string>();
      List<double> categoryTotalList = new List<double>();

      int expenseCount = data.ExpenseList.Count;
      for (int expenseIndex = 0; expenseIndex < expenseCount; expenseIndex++) {
        Expense expense = data.ExpenseList[expenseIndex];
        int categoryIndex = FindCategoryIndex(categoryNameList, expense.CategoryName);
        if (categoryIndex < 0) {
          categoryNameList.Add(expense.CategoryName);
          categoryTotalList.Add(expense.Amount);
        } else {
          double oldTotal = categoryTotalList[categoryIndex];
          double newTotal = oldTotal + expense.Amount;
          categoryTotalList[categoryIndex] = newTotal;
        }
      }

      double grandTotal = 0.0;
      int categoryCount = categoryNameList.Count;
      for (int categoryIndex = 0; categoryIndex < categoryCount; categoryIndex++) {
        string categoryName = categoryNameList[categoryIndex];
        double categoryTotal = categoryTotalList[categoryIndex];
        builder.AppendLine(categoryName + " : " + categoryTotal.ToString("0.00"));
        grandTotal = grandTotal + categoryTotal;
      }

      builder.AppendLine("----------------------------------------");
      builder.AppendLine("Grand total: " + grandTotal.ToString("0.00"));
      builder.AppendLine("========================================");
      return builder.ToString();
    }

    private static int FindCategoryIndex(List<string> categoryNameList, string categoryName) {
      int categoryCount = categoryNameList.Count;
      for (int categoryIndex = 0; categoryIndex < categoryCount; categoryIndex++) {
        if (categoryNameList[categoryIndex] == categoryName) {
          return categoryIndex;
        }
      }
      return -1;
    }
  }
}
