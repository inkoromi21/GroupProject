using System.Collections.Generic;
using Budget_App.Models;

namespace Budget_App.Services {
  /// <summary>
  /// Expense tracking for the active budget.
  /// </summary>
  internal interface IExpenseService {
    bool TryAddExpense(double amount, string categoryName, string description, out string errorMessage);

    List<Expense> GetExpensesForActiveBudget();

    bool TryDeleteExpense(int expenseId, out string errorMessage);
  }
}
