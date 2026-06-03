using Budget_App.AppConstants;
using Budget_App.Budgets;
using Budget_App.ConsoleUI;
using Budget_App.Stores;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget_App.Tests.Stores {
  [TestClass]
  public class BudgetStoreTests {
    [TestMethod]
    public void CreateTemplate_Personal_ReturnsPersonalTemplate() {
      BudgetStore budgetStore = new SimpleBudgetStore();
      BudgetTemplate template = budgetStore.GetTemplate(BudgetType.Personal);
      Assert.IsNotNull(template);
      Assert.AreEqual(BudgetConstants.PersonalDefaultLimit, template.GetDefaultLimit(), 0.001);
    }

    [TestMethod]
    public void CreateTemplate_Family_ReturnsFamilyTemplate() {
      BudgetStore budgetStore = new SimpleBudgetStore();
      BudgetTemplate template = budgetStore.GetTemplate(BudgetType.Family);
      Assert.IsNotNull(template);
      Assert.AreEqual(BudgetConstants.FamilyDefaultLimit, template.GetDefaultLimit(), 0.001);
    }

    [TestMethod]
    public void CreateTemplate_Business_ReturnsBusinessTemplate() {
      BudgetStore budgetStore = new SimpleBudgetStore();
      BudgetTemplate template = budgetStore.GetTemplate(BudgetType.Business);
      Assert.IsNotNull(template);
      Assert.AreEqual(BudgetConstants.BusinessDefaultLimit, template.GetDefaultLimit(), 0.001);
    }

    [TestMethod]
    public void CreateTemplate_Unknown_ReturnsNull() {
      BudgetStore budgetStore = new SimpleBudgetStore();
      BudgetTemplate template = budgetStore.GetTemplate(BudgetType.Unknown);
      Assert.IsNull(template);
    }
  }
}
