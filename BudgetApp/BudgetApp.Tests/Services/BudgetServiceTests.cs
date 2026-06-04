using Budget_App.ConsoleUI;
using Budget_App.Data;
using Budget_App.Models;
using Budget_App.Services;
using Budget_App.Stores;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget_App.Tests.Services {
  [TestClass]
  public class BudgetServiceTests {
    [TestMethod]
    public void Create_SavesBudgetToDatabase() {
      string connectionString = TestDatabaseHelper.CreateTempDatabase();
      BudgetRepository budgetRepository = new BudgetRepository(connectionString);
      BudgetStore budgetStore = new SimpleBudgetStore();
      BudgetService budgetService = new BudgetService(budgetRepository, budgetStore, null);

      Budget created = budgetService.Create("TestBudget", BudgetType.Personal, 50000.0);
      Assert.IsNotNull(created);
      Assert.IsTrue(created.Id > 0);
      Assert.AreEqual("TestBudget", created.Name);
      Assert.AreEqual(50000.0, created.TotalLimit, 0.001);
    }

    [TestMethod]
    public void SetActive_MarksBudgetAsActive() {
      string connectionString = TestDatabaseHelper.CreateTempDatabase();
      BudgetRepository budgetRepository = new BudgetRepository(connectionString);
      BudgetStore budgetStore = new SimpleBudgetStore();
      BudgetService budgetService = new BudgetService(budgetRepository, budgetStore, null);

      Budget created = budgetService.Create("", BudgetType.Family, -1.0);
      bool isSet = budgetService.SetActive(created.Id);
      Assert.IsTrue(isSet);

      Budget activeBudget = budgetService.GetActiveBudget();
      Assert.IsNotNull(activeBudget);
      Assert.AreEqual(created.Id, activeBudget.Id);
      Assert.IsTrue(activeBudget.IsActive);
    }
  }
}
