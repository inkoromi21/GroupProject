using System;

namespace Budget_App.Models {
  internal class SavingsGoal {
    public int Id;
    public int BudgetId;
    public string Name;
    public double TargetAmount;
    public double CurrentAmount;
    public DateTime Deadline;
  }
}
