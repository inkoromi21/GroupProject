using Budget_App.Budgets;
using Budget_App.ConsoleUI;

namespace Budget_App.Stores {
  internal class SimpleBudgetStore : BudgetStore {
    protected override BudgetTemplate CreateTemplate(BudgetType type) {
      switch (type) {
        case BudgetType.Personal:
          return new PersonalBudgetTemplate();
        case BudgetType.Family:
          return new FamilyBudgetTemplate();
        case BudgetType.Business:
          return new BusinessBudgetTemplate();
        default:
          return null;
      }
    }
  }
}
