using System.Collections.Generic;
using Budget_App.Models;

namespace Budget_App.Reports {
  /// <summary>
  /// Input data for budget report strategies.
  /// </summary>
  internal class ReportData {
    public Budget Budget;
    public List<Expense> ExpenseList;
    public List<SavingsGoal> SavingsGoalList;

    public ReportData() {
      ExpenseList = new List<Expense>();
      SavingsGoalList = new List<SavingsGoal>();
    }
  }
}
