using Budget_App.Budgets;
using Budget_App.ConsoleUI;

namespace Budget_App.Stores {
  internal abstract class BudgetStore {
    public BudgetTemplate GetTemplate(BudgetType type) {
      BudgetTemplate template = CreateTemplate(type);
      return template;
    }

    protected abstract BudgetTemplate CreateTemplate(BudgetType type);
  }
}
