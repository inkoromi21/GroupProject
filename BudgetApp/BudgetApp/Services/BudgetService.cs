using System;
using System.Collections.Generic;
using Budget_App.AppConstants;
using Budget_App.Budgets;
using Budget_App.ConsoleUI;
using Budget_App.Data;
using Budget_App.Models;
using Budget_App.Observers;
using Budget_App.Stores;

namespace Budget_App.Services {
  internal class BudgetService : IBudgetService {
    private readonly BudgetRepository budgetRepository;
    private readonly BudgetStore budgetStore;
    private readonly IBudgetSubject budgetSubject;

    public BudgetService(
      BudgetRepository budgetRepository,
      BudgetStore budgetStore,
      IBudgetSubject budgetSubject) {
      this.budgetRepository = budgetRepository;
      this.budgetStore = budgetStore;
      this.budgetSubject = budgetSubject;
    }

    public Budget Create(string name, BudgetType type, double customLimit) {
      BudgetTemplate template;
      template = budgetStore.GetTemplate(type);
      if (template == null) {
        return null;
      }

      double limit;
      if (customLimit < 0.0) {
        limit = template.GetDefaultLimit();
      } else {
        limit = customLimit;
      }

      if (!template.ValidateLimit(limit)) {
        return null;
      }

      string budgetName;
      if (name == null || name.Trim().Length == 0) {
        budgetName = template.GetDefaultName();
      } else {
        budgetName = name.Trim();
      }

      DateTime periodStart;
      periodStart = DateTime.Today;

      DateTime periodEnd;
      periodEnd = periodStart.AddMonths(BudgetConstants.BudgetPeriodMonths);

      Budget budget;
      budget = new Budget();
      budget.Name = budgetName;
      budget.Type = type.ToString();
      budget.TotalLimit = limit;
      budget.PeriodStart = periodStart;
      budget.PeriodEnd = periodEnd;
      budget.CreatedAt = DateTime.UtcNow;
      budget.IsActive = false;

      int newId;
      newId = budgetRepository.Add(budget);
      budget.Id = newId;

      if (budgetSubject != null) {
        BudgetEventArgs eventArgs;
        eventArgs = new BudgetEventArgs();
        eventArgs.message = "Создан бюджет: " + budget.Name;
        eventArgs.budgetId = budget.Id;
        eventArgs.eventType = BudgetEventType.BudgetCreated;
        budgetSubject.Notify(eventArgs);
      }

      return budget;
    }

    public List<Budget> GetAll() {
      List<Budget> budgetList;
      budgetList = budgetRepository.GetAll();
      return budgetList;
    }

    public bool SetActive(int budgetId) {
      Budget existing;
      existing = budgetRepository.GetById(budgetId);
      if (existing == null) {
        return false;
      }

      budgetRepository.SetActive(budgetId);

      if (budgetSubject != null) {
        BudgetEventArgs eventArgs;
        eventArgs = new BudgetEventArgs();
        eventArgs.message = "Активный бюджет: Id=" + budgetId;
        eventArgs.budgetId = budgetId;
        eventArgs.eventType = BudgetEventType.ActiveBudgetChanged;
        budgetSubject.Notify(eventArgs);
      }

      return true;
    }

    public Budget GetActive() {
      Budget activeBudget;
      activeBudget = budgetRepository.GetActive();
      return activeBudget;
    }

    public Budget GetActiveBudget() {
      Budget activeBudget;
      activeBudget = GetActive();
      return activeBudget;
    }
  }
}
