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

      BudgetRepository budgetRepository = new BudgetRepository(connectionString);
      BudgetService budgetService = new BudgetService(budgetRepository);
      BudgetController budgetController = new BudgetController(budgetService);

      while (true) {
        string action = ConsoleMenu.ReadAction();
        Console.WriteLine();

        if (action == "0") {
          Console.WriteLine("До свидания!");
          return;
        }

        if (action == "1") {
          budgetController.CreateBudget();
          continue;
        }

        if (action == "2") {
          budgetController.SelectActiveBudget();
          continue;
        }

        if (action == "3" || action == "4" || action == "5" || action == "6" || action == "7") {
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
