using System;
using System.Collections.Generic;
using Budget_App.Models;
using Budget_App.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget_App.Tests.Reports {
  [TestClass]
  public class ByCategoryReportStrategyTests {
    [TestMethod]
    public void Generate_TwoCategories_BothAppearInText() {
      Budget budget = new Budget();
      budget.Name = "Test";
      budget.TotalLimit = 1000.0;

      Expense expenseOne = new Expense();
      expenseOne.CategoryName = "Food";
      expenseOne.Amount = 100.0;
      expenseOne.Date = DateTime.Now;

      Expense expenseTwo = new Expense();
      expenseTwo.CategoryName = "Transport";
      expenseTwo.Amount = 50.0;
      expenseTwo.Date = DateTime.Now;

      List<Expense> expenseList = new List<Expense>();
      expenseList.Add(expenseOne);
      expenseList.Add(expenseTwo);

      ReportData reportData = new ReportData();
      reportData.Budget = budget;
      reportData.ExpenseList = expenseList;

      ByCategoryReportStrategy strategy = new ByCategoryReportStrategy();
      string text = strategy.Generate(reportData);

      Assert.IsTrue(text.Contains("Food"));
      Assert.IsTrue(text.Contains("Transport"));
    }
  }
}
