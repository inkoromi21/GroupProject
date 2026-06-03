using Budget_App.Data;
using Budget_App.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget_App.Tests.Services {
  [TestClass]
  public class ExpenseServiceTests {
    [TestMethod]
    public void AddExpense_WithoutActiveBudget_ReturnsFalse() {
      string connectionString = TestDatabaseHelper.CreateTempDatabase();
      TestDatabaseHelper.InsertInactiveBudgetOnly(connectionString);

      IRepository repository = new SqliteRepository(connectionString);
      IBudgetService budgetService = new BudgetService(repository);
      ExpenseService expenseService = new ExpenseService(repository, budgetService);

      string errorMessage = "";
      bool saved = expenseService.TryAddExpense(100.0, "Food", "test", out errorMessage);

      Assert.IsFalse(saved);
      Assert.IsTrue(errorMessage.Length > 0);
    }

    [TestMethod]
    public void AddExpense_WithActiveBudget_SavesExpense() {
      string connectionString = TestDatabaseHelper.CreateTempDatabase();
      TestDatabaseHelper.InsertActiveBudget(connectionString, "TestBudget", 10000.0);

      IRepository repository = new SqliteRepository(connectionString);
      IBudgetService budgetService = new BudgetService(repository);
      ExpenseService expenseService = new ExpenseService(repository, budgetService);

      string errorMessage = "";
      bool saved = expenseService.TryAddExpense(250.0, "Transport", "bus", out errorMessage);

      Assert.IsTrue(saved);
      System.Collections.Generic.List<Budget_App.Models.Expense> list =
        expenseService.GetExpensesForActiveBudget();
      Assert.AreEqual(1, list.Count);
      Assert.AreEqual(250.0, list[0].Amount, 0.001);
    }
  }
}
