using System;
using System.Collections.Generic;
using Budget_App.AppConstants;
using Budget_App.Budgets;
using Budget_App.ConsoleUI;
using Budget_App.Data;
using Budget_App.Models;
using Budget_App.Stores;

namespace Budget_App.Services {
  internal class BudgetService {
    private readonly BudgetRepository budgetRepository;

    public BudgetService(BudgetRepository budgetRepository) {
      this.budgetRepository = budgetRepository;
    }

    public Budget Create(string name, BudgetType type, double customLimit) {
      BudgetTemplate template = BudgetFactory.Create(type);
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

      DateTime periodStart = DateTime.Today;
      DateTime periodEnd = periodStart.AddMonths(BudgetConstants.BudgetPeriodMonths);

      Budget budget = new Budget();
      budget.Name = budgetName;
      budget.Type = type.ToString();
      budget.TotalLimit = limit;
      budget.PeriodStart = periodStart;
      budget.PeriodEnd = periodEnd;
      budget.CreatedAt = DateTime.UtcNow;
      budget.IsActive = false;

      int newId = budgetRepository.Add(budget);
      budget.Id = newId;
      return budget;
    }

    public List<Budget> GetAll() {
      return budgetRepository.GetAll();
    }

    public bool SetActive(int budgetId) {
      Budget existing = budgetRepository.GetById(budgetId);
      if (existing == null) {
        return false;
      }
      budgetRepository.SetActive(budgetId);
      return true;
    }

    public Budget GetActive() {
      return budgetRepository.GetActive();
    }
  }
}
