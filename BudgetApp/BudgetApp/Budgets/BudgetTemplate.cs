namespace Budget_App.Budgets {
  internal abstract class BudgetTemplate {
    public abstract string GetDefaultName();
    public abstract double GetDefaultLimit();
    public abstract bool ValidateLimit(double limit);
  }
}
