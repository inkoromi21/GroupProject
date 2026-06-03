using System.Collections.Generic;
using Budget_App.Models;

namespace Budget_App.Data {
  /// <summary>
  /// Data access for budgets, expenses, and savings goals.
  /// </summary>
  internal interface IRepository {
    Budget GetActiveBudget();

    Budget GetBudgetById(int budgetId);

    List<Budget> GetAllBudgets();

    int AddExpense(Expense expense);

    List<Expense> GetExpensesByBudgetId(int budgetId);

    Expense GetExpenseById(int expenseId);

    bool DeleteExpense(int expenseId);

    List<SavingsGoal> GetSavingsGoalsByBudgetId(int budgetId);
  }
}
