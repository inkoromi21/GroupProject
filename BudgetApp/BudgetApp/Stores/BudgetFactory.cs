using System;
using Budget_App.AppConstants;
using Budget_App.Models;

namespace Budget_App.Stores {
  internal class BudgetFactory {
    public Budget CreateBudget(int budgetTypeCode, string name, double totalLimit, DateTime periodStart, DateTime periodEnd) {
      Budget budget = new Budget();
      budget.Name = name;
      budget.TotalLimit = totalLimit;
      budget.PeriodStart = periodStart;
      budget.PeriodEnd = periodEnd;
      budget.CreatedAt = DateTime.Now;
      budget.IsActive = false;

      if (budgetTypeCode == BudgetConstants.budgetTypePersonal) {
        budget.Type = "Personal";
      } else if (budgetTypeCode == BudgetConstants.budgetTypeFamily) {
        budget.Type = "Family";
      } else if (budgetTypeCode == BudgetConstants.budgetTypeProject) {
        budget.Type = "Project";
      } else {
        budget.Type = "Personal";
      }

      return budget;
    }
  }
}
