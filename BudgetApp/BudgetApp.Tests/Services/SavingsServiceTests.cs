using System;
using System.Collections.Generic;
using Budget_App.Data;
using Budget_App.Models;
using Budget_App.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget_App.Tests.Services {
  [TestClass]
  public class SavingsServiceTests {
    [TestMethod]
    public void CreateGoal_WithActiveBudget_Works() {
      string connectionString;
      connectionString = TestDatabaseHelper.CreateTempDatabase();

      string budgetName;
      budgetName = "TestBudget";

      double budgetLimit;
      budgetLimit = 10000.0;

      TestDatabaseHelper.InsertActiveBudget(connectionString, budgetName, budgetLimit);

      SavingsRepository savingsRepository;
      savingsRepository = new SavingsRepository(connectionString);

      SavingsService savingsService;
      savingsService = new SavingsService(savingsRepository, null);

      string errorMessage;
      errorMessage = "";

      string goalName;
      goalName = "Phone";

      double targetAmount;
      targetAmount = 3000.0;

      DateTime deadline;
      deadline = new DateTime(2026, 12, 1);

      bool isOk;
      isOk = savingsService.TryCreateGoal(goalName, targetAmount, deadline, out errorMessage);

      Assert.IsTrue(isOk, errorMessage);

      List<SavingsGoal> goalList;
      goalList = savingsService.GetGoalsForActiveBudget();

      int goalCount;
      goalCount = goalList.Count;

      int expectedCount;
      expectedCount = 1;

      Assert.AreEqual(expectedCount, goalCount);
    }

    [TestMethod]
    public void AddMoney_IncreasesCurrentAmount() {
      string connectionString;
      connectionString = TestDatabaseHelper.CreateTempDatabase();

      string budgetName;
      budgetName = "TestBudget";

      double budgetLimit;
      budgetLimit = 10000.0;

      TestDatabaseHelper.InsertActiveBudget(connectionString, budgetName, budgetLimit);

      SavingsRepository savingsRepository;
      savingsRepository = new SavingsRepository(connectionString);

      SavingsService savingsService;
      savingsService = new SavingsService(savingsRepository, null);

      string errorMessage;
      errorMessage = "";

      string goalName;
      goalName = "Phone";

      double targetAmount;
      targetAmount = 1000.0;

      DateTime deadline;
      deadline = new DateTime(2026, 12, 1);

      savingsService.TryCreateGoal(goalName, targetAmount, deadline, out errorMessage);

      List<SavingsGoal> goalList;
      goalList = savingsService.GetGoalsForActiveBudget();

      int firstGoalIndex;
      firstGoalIndex = 0;

      int goalId;
      goalId = goalList[firstGoalIndex].Id;

      double addAmount;
      addAmount = 200.0;

      bool isOk;
      isOk = savingsService.TryAddMoney(goalId, addAmount, out errorMessage);

      Assert.IsTrue(isOk, errorMessage);

      SavingsGoal goal;
      goal = savingsRepository.GetById(goalId);

      double expectedAmount;
      expectedAmount = 200.0;

      double delta;
      delta = 0.001;

      Assert.AreEqual(expectedAmount, goal.CurrentAmount, delta);
    }
  }
}
