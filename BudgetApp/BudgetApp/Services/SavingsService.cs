using System;
using System.Collections.Generic;
using Budget_App.Data;
using Budget_App.Models;
using Budget_App.Observers;

namespace Budget_App.Services {
  internal class SavingsService {
    private readonly SavingsRepository savingsRepository;
    private readonly IBudgetSubject budgetSubject;

    public SavingsService(SavingsRepository savingsRepository, IBudgetSubject budgetSubject) {
      this.savingsRepository = savingsRepository;
      this.budgetSubject = budgetSubject;
    }

    public bool TryCreateGoal(string name, double targetAmount, DateTime deadline, out string errorMessage) {
      errorMessage = "";

      Budget activeBudget;
      activeBudget = savingsRepository.GetActiveBudget();
      if (activeBudget == null) {
        errorMessage = "Нет активного бюджета. Сначала пункт 2.";
        return false;
      }

      bool nameIsEmpty;
      if (name == null) {
        nameIsEmpty = true;
      } else {
        nameIsEmpty = name.Trim().Length == 0;
      }
      if (nameIsEmpty) {
        errorMessage = "Введите название цели.";
        return false;
      }

      if (targetAmount <= 0.0) {
        errorMessage = "Сумма цели должна быть больше нуля.";
        return false;
      }

      SavingsGoal goal;
      goal = new SavingsGoal();
      goal.BudgetId = activeBudget.Id;
      goal.Name = name.Trim();
      goal.TargetAmount = targetAmount;
      goal.CurrentAmount = 0.0;
      goal.Deadline = deadline;

      int newId;
      newId = savingsRepository.Add(goal);
      goal.Id = newId;

      if (budgetSubject != null) {
        BudgetEventArgs eventArgs;
        eventArgs = new BudgetEventArgs();
        eventArgs.message = "Создана цель: " + goal.Name;
        eventArgs.budgetId = activeBudget.Id;
        eventArgs.eventType = BudgetEventType.SavingsGoalCreated;
        budgetSubject.Notify(eventArgs);
      }

      return true;
    }

    public bool TryAddMoney(int goalId, double amount, out string errorMessage) {
      errorMessage = "";

      Budget activeBudget;
      activeBudget = savingsRepository.GetActiveBudget();
      if (activeBudget == null) {
        errorMessage = "Нет активного бюджета. Сначала пункт 2.";
        return false;
      }

      if (amount <= 0.0) {
        errorMessage = "Сумма должна быть больше нуля.";
        return false;
      }

      SavingsGoal goal;
      goal = savingsRepository.GetById(goalId);
      if (goal == null) {
        errorMessage = "Цель не найдена.";
        return false;
      }

      if (goal.BudgetId != activeBudget.Id) {
        errorMessage = "Эта цель другого бюджета.";
        return false;
      }

      double newAmount;
      newAmount = goal.CurrentAmount + amount;

      if (newAmount > goal.TargetAmount) {
        double leftToGoal;
        leftToGoal = goal.TargetAmount - goal.CurrentAmount;
        errorMessage = "Слишком много. Осталось: " + leftToGoal.ToString("0.00");
        return false;
      }

      bool isUpdated;
      isUpdated = savingsRepository.UpdateCurrentAmount(goalId, newAmount);
      if (!isUpdated) {
        errorMessage = "Не удалось сохранить.";
        return false;
      }

      if (budgetSubject != null) {
        double percent;
        percent = GetProgressPercent(goalId);

        BudgetEventArgs eventArgs;
        eventArgs = new BudgetEventArgs();
        eventArgs.message = "Пополнено: " + goal.Name + ". Прогресс " + percent.ToString("0.0") + "%";
        eventArgs.budgetId = activeBudget.Id;
        eventArgs.eventType = BudgetEventType.SavingsUpdated;
        budgetSubject.Notify(eventArgs);
      }

      return true;
    }

    public List<SavingsGoal> GetGoalsForActiveBudget() {
      Budget activeBudget;
      activeBudget = savingsRepository.GetActiveBudget();
      if (activeBudget == null) {
        List<SavingsGoal> emptyList;
        emptyList = new List<SavingsGoal>();
        return emptyList;
      }

      List<SavingsGoal> goalList;
      goalList = savingsRepository.GetByBudgetId(activeBudget.Id);
      return goalList;
    }

    public double GetProgressPercent(int goalId) {
      SavingsGoal goal;
      goal = savingsRepository.GetById(goalId);
      if (goal == null) {
        return 0.0;
      }

      if (goal.TargetAmount <= 0.0) {
        return 0.0;
      }

      double ratio;
      ratio = goal.CurrentAmount / goal.TargetAmount;

      double percent;
      percent = ratio * 100.0;
      return percent;
    }
  }
}
