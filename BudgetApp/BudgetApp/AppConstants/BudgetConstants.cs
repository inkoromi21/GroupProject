namespace Budget_App.AppConstants {
  internal static class BudgetConstants {
    public static readonly double PersonalDefaultLimit = 30000.0;
    public static readonly double PersonalMaxLimit = 500000.0;

    public static readonly double FamilyDefaultLimit = 80000.0;
    public static readonly double FamilyMaxLimit = 1000000.0;

    public static readonly double BusinessDefaultLimit = 150000.0;
    public static readonly double BusinessMinLimit = 10000.0;
    public static readonly double BusinessMaxLimit = 5000000.0;

    public static readonly int BudgetPeriodMonths = 1;
    public static readonly double NoCustomLimit = -1.0;
  }
}
