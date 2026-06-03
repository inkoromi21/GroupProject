using System;
using Budget_App.ConsoleUI;
using Budget_App.Controllers;
using Budget_App.Data;
using Budget_App.Services;

namespace Budget_App {
  internal class Program {
    private const string databaseFileName = "budgetapp.db";

    private static void Main(string[] args) {
      Console.OutputEncoding = System.Text.Encoding.UTF8;
      Console.Title = "BudgetApp";

      string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, databaseFileName);
      string connectionString = "Data Source=" + dbPath + ";Version=3;";
      DatabaseSchemaInitializer.EnsureCreated(connectionString);

      IRepository repository = new SqliteRepository(connectionString);
      IBudgetService budgetService = new BudgetService(repository);
      IExpenseService expenseService = new ExpenseService(repository, budgetService);

      ExpenseController expenseController = new ExpenseController(expenseService);
      ReportController reportController = new ReportController(repository, budgetService);

      string menuCodeExit = ((int)MenuOption.Exit).ToString();
      string menuCodeAddExpense = ((int)MenuOption.AddExpense).ToString();
      string menuCodeListExpenses = ((int)MenuOption.ListExpenses).ToString();
      string menuCodeBudgetReport = ((int)MenuOption.BudgetReport).ToString();
      string menuCodeCreateBudget = ((int)MenuOption.CreateBudget).ToString();
      string menuCodeSelectBudget = ((int)MenuOption.SelectActiveBudget).ToString();
      string menuCodeSavingsGoals = ((int)MenuOption.SavingsGoals).ToString();
      string menuCodeAddSavings = ((int)MenuOption.AddToSavings).ToString();

      while (true) {
        string action = ConsoleMenu.ReadAction();
        Console.WriteLine();

        if (action == menuCodeExit) {
          Console.WriteLine("До свидания!");
          return;
        }

        if (action == menuCodeAddExpense) {
          expenseController.AddExpense();
          Console.WriteLine();
          continue;
        }

        if (action == menuCodeListExpenses) {
          expenseController.ListExpenses();
          Console.WriteLine();
          continue;
        }

        if (action == menuCodeBudgetReport) {
          reportController.ShowReportMenu();
          Console.WriteLine();
          continue;
        }

        if (action == menuCodeCreateBudget || action == menuCodeSelectBudget
          || action == menuCodeSavingsGoals || action == menuCodeAddSavings) {
          Console.WriteLine("Раздел в разработке.");
          Console.WriteLine();
          continue;
        }

        Console.WriteLine("Неизвестная команда. Попробуйте снова.");
        Console.WriteLine();
      }
    }
  }
}
