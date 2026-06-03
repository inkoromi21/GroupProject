using Budget_App.AppConstants;

namespace Budget_App.ConsoleUI {
  internal static class BudgetType {
    public static string GetDisplayName(int typeCode) {
      if (typeCode == BudgetConstants.budgetTypePersonal) {
        return "Personal";
      }
      if (typeCode == BudgetConstants.budgetTypeFamily) {
        return "Family";
      }
      if (typeCode == BudgetConstants.budgetTypeProject) {
        return "Project";
      }
      return "Unknown";
    }
  }
}
