using System;
using System.Collections.Generic;
using Budget_App.Data;
using Budget_App.Models;

namespace Budget_App.Services {
  /// <summary>
  /// Business logic for expense records.
  /// </summary>
  internal class ExpenseService : IExpenseService {
    private readonly IRepository repository;
    private readonly IBudgetService budgetService;

    public ExpenseService(IRepository repository, IBudgetService budgetService) {
      this.repository = repository;
      this.budgetService = budgetService;
    }

    /// <inheritdoc />
    public bool TryAddExpense(double amount, string categoryName, string description, out string errorMessage) {
      errorMessage = "";
      Budget activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        errorMessage = "No active budget. Select an active budget first (menu item 2).";
        return false;
      }

      if (amount <= 0.0) {
        errorMessage = "Amount must be greater than zero.";
        return false;
      }

      if (string.IsNullOrWhiteSpace(categoryName)) {
        errorMessage = "Category name is required.";
        return false;
      }

      Expense expense = new Expense();
      expense.BudgetId = activeBudget.Id;
      expense.Amount = amount;
      expense.CategoryName = categoryName.Trim();
      expense.Date = DateTime.Now;
      expense.Description = description == null ? "" : description.Trim();

      int newId = repository.AddExpense(expense);
      expense.Id = newId;
      return true;
    }

    /// <inheritdoc />
    public List<Expense> GetExpensesForActiveBudget() {
      Budget activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        return new List<Expense>();
      }
      List<Expense> expenseList = repository.GetExpensesByBudgetId(activeBudget.Id);
      return expenseList;
    }

    /// <inheritdoc />
    public bool TryDeleteExpense(int expenseId, out string errorMessage) {
      errorMessage = "";
      Budget activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        errorMessage = "No active budget. Select an active budget first (menu item 2).";
        return false;
      }

      Expense expense = repository.GetExpenseById(expenseId);
      if (expense == null) {
        errorMessage = "Expense not found.";
        return false;
      }

      if (expense.BudgetId != activeBudget.Id) {
        errorMessage = "Expense belongs to another budget.";
        return false;
      }

      bool deleted = repository.DeleteExpense(expenseId);
      if (!deleted) {
        errorMessage = "Could not delete expense.";
        return false;
      }
      return true;
    }
  }
}
