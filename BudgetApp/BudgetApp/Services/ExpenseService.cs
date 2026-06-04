using System;
using System.Collections.Generic;
using Budget_App.Data;
using Budget_App.Models;
using Budget_App.Observers;

namespace Budget_App.Services {
  internal class ExpenseService : IExpenseService {
    private readonly IRepository repository;
    private readonly IBudgetService budgetService;
    private readonly IBudgetSubject budgetSubject;

    public ExpenseService(
      IRepository repository,
      IBudgetService budgetService,
      IBudgetSubject budgetSubject) {
      this.repository = repository;
      this.budgetService = budgetService;
      this.budgetSubject = budgetSubject;
    }

    public bool TryAddExpense(double amount, string categoryName, string description, out string errorMessage) {
      errorMessage = "";

      Budget activeBudget;
      activeBudget = budgetService.GetActiveBudget();
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

      Expense expense;
      expense = new Expense();
      expense.BudgetId = activeBudget.Id;
      expense.Amount = amount;
      expense.CategoryName = categoryName.Trim();
      expense.Date = DateTime.Now;

      if (description == null) {
        expense.Description = "";
      } else {
        expense.Description = description.Trim();
      }

      int newId;
      newId = repository.AddExpense(expense);
      expense.Id = newId;

      if (budgetSubject != null) {
        BudgetEventArgs eventArgs;
        eventArgs = new BudgetEventArgs();
        eventArgs.message = "Добавлен расход: " + expense.CategoryName + ", сумма " + expense.Amount;
        eventArgs.budgetId = activeBudget.Id;
        eventArgs.eventType = BudgetEventType.ExpenseAdded;
        budgetSubject.Notify(eventArgs);
      }

      return true;
    }

    public List<Expense> GetExpensesForActiveBudget() {
      Budget activeBudget;
      activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        List<Expense> emptyList;
        emptyList = new List<Expense>();
        return emptyList;
      }

      List<Expense> expenseList;
      expenseList = repository.GetExpensesByBudgetId(activeBudget.Id);
      return expenseList;
    }

    public bool TryDeleteExpense(int expenseId, out string errorMessage) {
      errorMessage = "";

      Budget activeBudget;
      activeBudget = budgetService.GetActiveBudget();
      if (activeBudget == null) {
        errorMessage = "Нет активного бюджета. Сначала выберите бюджет (пункт 2).";
        return false;
      }

      Expense expense;
      expense = repository.GetExpenseById(expenseId);
      if (expense == null) {
        errorMessage = "Расход не найден.";
        return false;
      }

      if (expense.BudgetId != activeBudget.Id) {
        errorMessage = "Расход относится к другому бюджету.";
        return false;
      }

      bool deleted;
      deleted = repository.DeleteExpense(expenseId);
      if (!deleted) {
        errorMessage = "Не удалось удалить расход.";
        return false;
      }

      if (budgetSubject != null) {
        BudgetEventArgs eventArgs;
        eventArgs = new BudgetEventArgs();
        eventArgs.message = "Удалён расход Id=" + expenseId;
        eventArgs.budgetId = activeBudget.Id;
        eventArgs.eventType = BudgetEventType.ExpenseDeleted;
        budgetSubject.Notify(eventArgs);
      }

      return true;
    }
  }
}
