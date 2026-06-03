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

      BudgetRepository budgetRepository = new BudgetRepository(connectionString);
      IRepository repository = new SqliteRepository(connectionString);
      BudgetService budgetService = new BudgetService(budgetRepository);
      BudgetController budgetController = new BudgetController(budgetService);
      IExpenseService expenseService = new ExpenseService(repository, budgetService);
      ExpenseController expenseController = new ExpenseController(expenseService);
      ReportController reportController = new ReportController(repository, budgetService);

      string menuCodeExit = ((int)MenuOption.Exit).ToString();
      string menuCodeCreateBudget = ((int)MenuOption.CreateBudget).ToString();
      string menuCodeSelectBudget = ((int)MenuOption.SelectActiveBudget).ToString();
      string menuCodeAddExpense = ((int)MenuOption.AddExpense).ToString();
      string menuCodeListExpenses = ((int)MenuOption.ListExpenses).ToString();
      string menuCodeSavingsGoals = ((int)MenuOption.SavingsGoals).ToString();
      string menuCodeAddSavings = ((int)MenuOption.AddToSavings).ToString();
      string menuCodeBudgetReport = ((int)MenuOption.BudgetReport).ToString();

      while (true) {
        string action = ConsoleMenu.ReadAction();
        Console.WriteLine();

        if (action == menuCodeExit) {
          Console.WriteLine("До свидания!");
          return;
        }

        if (action == menuCodeCreateBudget) {
          budgetController.CreateBudget();
          Console.WriteLine();
          continue;
        }

        if (action == menuCodeSelectBudget) {
          budgetController.SelectActiveBudget();
          Console.WriteLine();
          continue;
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

        if (action == menuCodeSavingsGoals || action == menuCodeAddSavings) {
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
