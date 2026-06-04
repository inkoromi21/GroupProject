using System;
using Budget_App.ConsoleUI;
using Budget_App.Controllers;
using Budget_App.Data;
using Budget_App.Observers;
using Budget_App.Services;
using Budget_App.Stores;

namespace Budget_App {
  internal class Program {
    private const string databaseFileName = "budgetapp.db";

    private static void Main(string[] args) {
      Console.OutputEncoding = System.Text.Encoding.UTF8;
      Console.Title = "BudgetApp";

      string dbPath;
      dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, databaseFileName);

      string connectionString;
      connectionString = "Data Source=" + dbPath + ";Version=3;";

      DatabaseSchemaInitializer.EnsureCreated(connectionString);

      IBudgetSubject budgetSubject;
      budgetSubject = new BudgetSubject();

      ConsoleBudgetObserver consoleObserver;
      consoleObserver = new ConsoleBudgetObserver();

      budgetSubject.Attach(consoleObserver);

      BudgetRepository budgetRepository;
      budgetRepository = new BudgetRepository(connectionString);

      IRepository repository;
      repository = new SqliteRepository(connectionString);

      SavingsRepository savingsRepository;
      savingsRepository = new SavingsRepository(connectionString);

      BudgetStore budgetStore;
      budgetStore = new SimpleBudgetStore();

      BudgetService budgetService;
      budgetService = new BudgetService(budgetRepository, budgetStore, budgetSubject);

      BudgetController budgetController;
      budgetController = new BudgetController(budgetService);

      IExpenseService expenseService;
      expenseService = new ExpenseService(repository, budgetService, budgetSubject);

      ExpenseController expenseController;
      expenseController = new ExpenseController(expenseService);

      ReportController reportController;
      reportController = new ReportController(repository, budgetService);

      SavingsService savingsService;
      savingsService = new SavingsService(savingsRepository, budgetSubject);

      SavingsController savingsController;
      savingsController = new SavingsController(savingsService);

      string menuCodeExit;
      menuCodeExit = ((int)MenuOption.Exit).ToString();

      string menuCodeCreateBudget;
      menuCodeCreateBudget = ((int)MenuOption.CreateBudget).ToString();

      string menuCodeSelectBudget;
      menuCodeSelectBudget = ((int)MenuOption.SelectActiveBudget).ToString();

      string menuCodeAddExpense;
      menuCodeAddExpense = ((int)MenuOption.AddExpense).ToString();

      string menuCodeListExpenses;
      menuCodeListExpenses = ((int)MenuOption.ListExpenses).ToString();

      string menuCodeSavingsGoals;
      menuCodeSavingsGoals = ((int)MenuOption.SavingsGoals).ToString();

      string menuCodeAddSavings;
      menuCodeAddSavings = ((int)MenuOption.AddToSavings).ToString();

      string menuCodeBudgetReport;
      menuCodeBudgetReport = ((int)MenuOption.BudgetReport).ToString();

      while (true) {
        string action;
        action = ConsoleMenu.ReadAction();
        Console.WriteLine();

        if (action == menuCodeExit) {
          Console.WriteLine("До свидания!");
          return;
        }

        Console.Clear();

        if (action == menuCodeCreateBudget) {
          budgetController.CreateBudget();
          continue;
        }

        if (action == menuCodeSelectBudget) {
          budgetController.SelectActiveBudget();
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

        if (action == menuCodeSavingsGoals) {
          savingsController.ShowGoalsMenu();
          continue;
        }

        if (action == menuCodeAddSavings) {
          savingsController.AddMoneyToGoal();
          continue;
        }

        if (action == menuCodeBudgetReport) {
          reportController.ShowReportMenu();
          Console.WriteLine();
          continue;
        }

        Console.WriteLine("Неизвестная команда. Попробуйте снова.");
        Console.WriteLine();
      }
    }
  }
}
