using System;
using Budget_App.Data;
using Budget_App.Models;
using Budget_App.Reports;
using Budget_App.Services;

namespace Budget_App.Controllers {
  internal class ReportController {
    private readonly IRepository repository;
    private readonly IBudgetService budgetService;

    private const string reportChoiceBack = "0";
    private const string reportChoiceMonthly = "1";
    private const string reportChoiceByCategory = "2";
    private const string reportChoiceSummary = "3";

    public ReportController(IRepository repository, IBudgetService budgetService) {
      this.repository = repository;
      this.budgetService = budgetService;
    }

    public void ShowReportMenu() {
      while (true) {
        Console.WriteLine("--- Бюджетный отчёт ---");
        Console.WriteLine("1) За период");
        Console.WriteLine("2) По категориям");
        Console.WriteLine("3) Сводка");
        Console.WriteLine("0) Назад");
        Console.Write("Ваш выбор: ");
        string line = Console.ReadLine();
        if (line == null) {
          line = "";
        }
        string choice = line.Trim();

        if (choice == reportChoiceBack) {
          return;
        }

        if (choice == reportChoiceMonthly || choice == reportChoiceByCategory || choice == reportChoiceSummary) {
          RunReport(choice);
          Console.WriteLine();
          continue;
        }

        Console.WriteLine("Неизвестный пункт.");
        Console.WriteLine();
      }
    }

    private void RunReport(string choice) {
      Budget activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        Console.WriteLine("Нет активного бюджета. Сначала выберите бюджет (пункт 2).");
        return;
      }

      ReportData reportData = new ReportData();
      reportData.Budget = activeBudget;
      reportData.ExpenseList = repository.GetExpensesByBudgetId(activeBudget.Id);
      reportData.SavingsGoalList = repository.GetSavingsGoalsByBudgetId(activeBudget.Id);

      ReportContext reportContext = new ReportContext(reportData);
      IReportStrategy strategy = null;
      if (choice == reportChoiceMonthly) {
        strategy = new MonthlyReportStrategy();
      } else if (choice == reportChoiceByCategory) {
        strategy = new ByCategoryReportStrategy();
      } else if (choice == reportChoiceSummary) {
        strategy = new SummaryReportStrategy();
      }

      if (strategy == null) {
        Console.WriteLine("Неизвестный тип отчёта.");
        return;
      }

      reportContext.SetStrategy(strategy);
      string reportText = reportContext.GenerateReport();
      Console.WriteLine(reportText);
    }
  }
}
