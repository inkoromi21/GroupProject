using System.Collections.Generic;
using Budget_App.Models;

namespace Budget_App.Reports {
  internal class ReportData {
    public Budget Budget;
    public List<Expense> ExpenseList;
    public List<SavingsGoal> SavingsGoalList;

    public ReportData() {
      Budget = null;
      ExpenseList = new List<Expense>();
      SavingsGoalList = new List<SavingsGoal>();
    }
  }
}
