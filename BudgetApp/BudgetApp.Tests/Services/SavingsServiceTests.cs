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

      TestDatabaseHelper.InsertActiveBudget(connectionString, "TestBudget", 10000.0);

      SavingsRepository savingsRepository;
      savingsRepository = new SavingsRepository(connectionString);

      SavingsService savingsService;
      savingsService = new SavingsService(savingsRepository, null);

      string errorMessage;
      errorMessage = "";

      DateTime deadline;
      deadline = new DateTime(2026, 12, 1);

      bool isOk;
      isOk = savingsService.TryCreateGoal("Phone", 3000.0, deadline, out errorMessage);

      Assert.IsTrue(isOk, errorMessage);

      List<SavingsGoal> goalList;
      goalList = savingsService.GetGoalsForActiveBudget();

      int goalCount;
      goalCount = goalList.Count;

      Assert.AreEqual(1, goalCount);
    }

    [TestMethod]
    public void AddMoney_IncreasesCurrentAmount() {
      string connectionString;
      connectionString = TestDatabaseHelper.CreateTempDatabase();

      TestDatabaseHelper.InsertActiveBudget(connectionString, "TestBudget", 10000.0);

      SavingsRepository savingsRepository;
      savingsRepository = new SavingsRepository(connectionString);

      SavingsService savingsService;
      savingsService = new SavingsService(savingsRepository, null);

      string errorMessage;
      errorMessage = "";

      DateTime deadline;
      deadline = new DateTime(2026, 12, 1);

      savingsService.TryCreateGoal("Phone", 1000.0, deadline, out errorMessage);

      List<SavingsGoal> goalList;
      goalList = savingsService.GetGoalsForActiveBudget();

      int goalId;
      goalId = goalList[0].Id;

      bool isOk;
      isOk = savingsService.TryAddMoney(goalId, 200.0, out errorMessage);

      Assert.IsTrue(isOk, errorMessage);

      SavingsGoal goal;
      goal = savingsRepository.GetById(goalId);

      double expectedAmount;
      expectedAmount = 200.0;

      Assert.AreEqual(expectedAmount, goal.CurrentAmount, 0.001);
    }
  }
}
