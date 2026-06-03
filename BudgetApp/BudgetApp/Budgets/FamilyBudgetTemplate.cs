using Budget_App.AppConstants;

namespace Budget_App.Budgets {
  internal class FamilyBudgetTemplate : BudgetTemplate {
    public override string GetDefaultName() {
      return "Семейный бюджет";
    }

    public override double GetDefaultLimit() {
      return BudgetConstants.FamilyDefaultLimit;
    }

    public override bool ValidateLimit(double limit) {
      return limit > 0.0 && limit <= BudgetConstants.FamilyMaxLimit;
    }
  }
}
