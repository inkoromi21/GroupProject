using System;

namespace Budget_App.Models {
  internal class Expense {
    public int Id;
    public int BudgetId;
    public double Amount;
    public string CategoryName;
    public DateTime Date;
    public string Description;
  }
}
