using System;
using Budget_App.ConsoleUI;
using Budget_App.Controllers;
using Budget_App.Data;
using Budget_App.Services;

namespace Budget_App {
  internal class Program {
    private const string DatabaseFileName = "budgetapp.db";

    private static void Main(string[] args) {
      Console.OutputEncoding = System.Text.Encoding.UTF8;
      Console.Title = "BudgetApp";

      string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);
      string connectionString = "Data Source=" + dbPath + ";Version=3;";
      DatabaseSchemaInitializer.EnsureCreated(connectionString);

      IRepository repository = new SqliteRepository(connectionString);
      IBudgetService budgetService = new BudgetService(repository);
      IExpenseService expenseService = new ExpenseService(repository, budgetService);

      ExpenseController expenseController = new ExpenseController(expenseService);
      ReportController reportController = new ReportController(repository, budgetService);

      while (true) {
        string action = ConsoleMenu.ReadAction();
        Console.WriteLine();

        if (action == "0") {
          Console.WriteLine("До свидания!");
          return;
        }

        if (action == "3") {
          expenseController.AddExpense();
          Console.WriteLine();
          continue;
        }

        if (action == "4") {
          expenseController.ListExpenses();
          Console.WriteLine();
          continue;
        }

        if (action == "7") {
          reportController.ShowReportMenu();
          Console.WriteLine();
          continue;
        }

        if (action == "1" || action == "2" || action == "5" || action == "6") {
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
