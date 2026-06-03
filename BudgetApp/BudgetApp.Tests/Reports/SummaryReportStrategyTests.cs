using System;
using System.Collections.Generic;
using Budget_App.Models;
using Budget_App.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget_App.Tests.Reports {
  [TestClass]
  public class SummaryReportStrategyTests {
    [TestMethod]
    public void Generate_ContainsLimitSpentAndBalance() {
      Budget budget = new Budget();
      budget.TotalLimit = 1000.0;

      Expense expense = new Expense();
      expense.Amount = 300.0;

      List<Expense> expenseList = new List<Expense>();
      expenseList.Add(expense);

      ReportData reportData = new ReportData();
      reportData.Budget = budget;
      reportData.ExpenseList = expenseList;

      SummaryReportStrategy strategy = new SummaryReportStrategy();
      string text = strategy.Generate(reportData);

      Assert.IsTrue(text.Contains("Лимит:"));
      Assert.IsTrue(text.Contains("Потрачено:"));
      Assert.IsTrue(text.Contains("Остаток:"));
      Assert.IsTrue(text.Contains("700.00"));
    }
  }
}
