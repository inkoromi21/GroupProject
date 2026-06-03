using System;
using Budget_App.Data;
using Budget_App.Models;
using Budget_App.Reports;
using Budget_App.Services;

namespace Budget_App.Controllers {
  /// <summary>
  /// Console flow for budget report menu item.
  /// </summary>
  internal class ReportController {
    private readonly IRepository repository;
    private readonly IBudgetService budgetService;

    public ReportController(IRepository repository, IBudgetService budgetService) {
      this.repository = repository;
      this.budgetService = budgetService;
    }

    /// <summary>
    /// Menu item 7: budget report submenu.
    /// </summary>
    public void ShowReportMenu() {
      while (true) {
        Console.WriteLine("--- Budget report ---");
        Console.WriteLine("1) For period (monthly)");
        Console.WriteLine("2) By category");
        Console.WriteLine("3) Summary");
        Console.WriteLine("0) Back");
        Console.Write("Your choice: ");
        string line = Console.ReadLine();
        if (line == null) {
          line = "";
        }
        string choice = line.Trim();

        if (choice == "0") {
          return;
        }

        if (choice == "1" || choice == "2" || choice == "3") {
          RunReport(choice);
          Console.WriteLine();
          continue;
        }

        Console.WriteLine("Unknown option.");
        Console.WriteLine();
      }
    }

    private void RunReport(string choice) {
      Budget activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        Console.WriteLine("No active budget. Select an active budget first (menu item 2).");
        return;
      }

      ReportData reportData = new ReportData();
      reportData.Budget = activeBudget;
      reportData.ExpenseList = repository.GetExpensesByBudgetId(activeBudget.Id);
      reportData.SavingsGoalList = repository.GetSavingsGoalsByBudgetId(activeBudget.Id);

      ReportContext reportContext = new ReportContext(reportData);
      IReportStrategy strategy = null;
      if (choice == "1") {
        strategy = new MonthlyReportStrategy();
      } else if (choice == "2") {
        strategy = new ByCategoryReportStrategy();
      } else if (choice == "3") {
        strategy = new SummaryReportStrategy();
      }

      if (strategy == null) {
        Console.WriteLine("Unknown report type.");
        return;
      }

      reportContext.SetStrategy(strategy);
      string reportText = reportContext.GenerateReport();
      Console.WriteLine(reportText);
    }
  }
}
