using Budget_App.AppConstants;

namespace Budget_App.Budgets {
  internal class PersonalBudgetTemplate : BudgetTemplate {
    public override string GetDefaultName() {
      return "Личный бюджет";
    }

    public override double GetDefaultLimit() {
      return BudgetConstants.PersonalDefaultLimit;
    }

    public override bool ValidateLimit(double limit) {
      return limit > 0.0 && limit <= BudgetConstants.PersonalMaxLimit;
    }
  }
}
