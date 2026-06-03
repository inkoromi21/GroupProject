using System;
using System.Collections.Generic;
using Budget_App.Data;
using Budget_App.Models;
using Budget_App.Stores;

namespace Budget_App.Services {
  internal class BudgetService : IBudgetService {
    private readonly BudgetRepository budgetRepository;
    private readonly BudgetFactory budgetFactory;

    public BudgetService(BudgetRepository budgetRepository) {
      this.budgetRepository = budgetRepository;
      budgetFactory = new BudgetFactory();
    }

    public void Create(string name, int budgetTypeCode, double totalLimit, DateTime periodStart, DateTime periodEnd) {
      Budget budget = budgetFactory.CreateBudget(budgetTypeCode, name, totalLimit, periodStart, periodEnd);
      int newId = budgetRepository.Add(budget);
      budget.Id = newId;
    }

    public List<Budget> GetAll() {
      List<Budget> budgetList = budgetRepository.GetAll();
      return budgetList;
    }

    public void SetActive(int budgetId) {
      budgetRepository.SetActive(budgetId);
    }

    public Budget GetActive() {
      Budget activeBudget = budgetRepository.GetActive();
      return activeBudget;
    }

    public Budget GetActiveBudget() {
      return GetActive();
    }
  }
}
