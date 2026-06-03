using System;
using System.Collections.Generic;
using Budget_App.Models;
using Budget_App.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget_App.Tests.Reports {
  [TestClass]
  public class MonthlyReportStrategyTests {
    [TestMethod]
    public void Generate_ReturnsTextWithBudgetNameAndPeriod() {
      Budget budget = new Budget();
      budget.Name = "HomeBudget";
      budget.PeriodStart = new DateTime(2026, 3, 1);
      budget.PeriodEnd = new DateTime(2026, 3, 31);

      ReportData reportData = new ReportData();
      reportData.Budget = budget;
      reportData.ExpenseList = new List<Expense>();

      MonthlyReportStrategy strategy = new MonthlyReportStrategy();
      string text = strategy.Generate(reportData);

      Assert.IsFalse(string.IsNullOrWhiteSpace(text));
      Assert.IsTrue(text.Contains("HomeBudget"));
      Assert.IsTrue(text.Contains("2026-03-01"));
      Assert.IsTrue(text.Contains("2026-03-31"));
    }
  }
}
