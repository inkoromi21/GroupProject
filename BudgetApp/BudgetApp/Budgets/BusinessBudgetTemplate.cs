using Budget_App.AppConstants;

namespace Budget_App.Budgets {
  internal class BusinessBudgetTemplate : BudgetTemplate {
    public override string GetDefaultName() {
      return "╨С╨╕╨╖╨╜╨╡╤Б-╨▒╤О╨┤╨╢╨╡╤В";
    }

    public override double GetDefaultLimit() {
      return BudgetConstants.BusinessDefaultLimit;
    }

    public override bool ValidateLimit(double limit) {
      return limit >= BudgetConstants.BusinessMinLimit
        && limit <= BudgetConstants.BusinessMaxLimit;
    }
  }
}
