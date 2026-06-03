using System;
using System.Collections.Generic;
using Budget_App.Data;
using Budget_App.Models;

namespace Budget_App.Services {
  internal class ExpenseService : IExpenseService {
    private readonly IRepository repository;
    private readonly IBudgetService budgetService;

    public ExpenseService(IRepository repository, IBudgetService budgetService) {
      this.repository = repository;
      this.budgetService = budgetService;
    }

    public bool TryAddExpense(double amount, string categoryName, string description, out string errorMessage) {
      errorMessage = "";
      Budget activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        errorMessage = "Нет активного бюджета. Сначала выберите бюджет (пункт 2).";
        return false;
      }

      if (amount <= 0.0) {
        errorMessage = "Сумма должна быть больше нуля.";
        return false;
      }

      if (string.IsNullOrWhiteSpace(categoryName)) {
        errorMessage = "Укажите категорию расхода.";
        return false;
      }

      Expense expense = new Expense();
      expense.BudgetId = activeBudget.Id;
      expense.Amount = amount;
      expense.CategoryName = categoryName.Trim();
      expense.Date = DateTime.Now;
      expense.Description = description == null ? "" : description.Trim();

      repository.AddExpense(expense);
      return true;
    }

    public List<Expense> GetExpensesForActiveBudget() {
      Budget activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        return new List<Expense>();
      }
      List<Expense> expenseList = repository.GetExpensesByBudgetId(activeBudget.Id);
      return expenseList;
    }

    public bool TryDeleteExpense(int expenseId, out string errorMessage) {
      errorMessage = "";
      Budget activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        errorMessage = "Нет активного бюджета. Сначала выберите бюджет (пункт 2).";
        return false;
      }

      Expense expense = repository.GetExpenseById(expenseId);
      if (expense == null) {
        errorMessage = "Расход не найден.";
        return false;
      }

      if (expense.BudgetId != activeBudget.Id) {
        errorMessage = "Расход относится к другому бюджету.";
        return false;
      }

      bool deleted = repository.DeleteExpense(expenseId);
      if (!deleted) {
        errorMessage = "Не удалось удалить расход.";
        return false;
      }
      return true;
    }
  }
}
