using Budget_App.Models;

namespace Budget_App.Stores {
  internal abstract class BudgetStore {
    protected abstract Budget CreateBudget(int budgetTypeCode);
  }
}
