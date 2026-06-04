using System;
using System.Collections.Generic;
using System.Globalization;
using Budget_App.Models;
using Budget_App.Services;

namespace Budget_App.Controllers {
  internal class SavingsController {
    private readonly SavingsService savingsService;

    public SavingsController(SavingsService savingsService) {
      this.savingsService = savingsService;
    }

    public void ShowGoalsMenu() {
      string menuCodeBack = "0";
      string menuCodeCreate = "1";
      string menuCodeList = "2";

      while (true) {
        Console.WriteLine("--- Цели сбережений ---");
        Console.WriteLine("1) Создать цель");
        Console.WriteLine("2) Список целей");
        Console.WriteLine("0) Назад");
        Console.Write("Выбор: ");

        string line;
        line = Console.ReadLine();
        if (line == null) {
          line = "";
        }
        string choice;
        choice = line.Trim();
        Console.WriteLine();

        if (choice == menuCodeBack) {
          return;
        }

        if (choice == menuCodeCreate) {
          CreateGoal();
          continue;
        }

        if (choice == menuCodeList) {
          PrintGoalList();
          continue;
        }

        Console.WriteLine("Неверный пункт.");
        Console.WriteLine();
      }
    }

    public void AddMoneyToGoal() {
      List<SavingsGoal> goalList;
      goalList = savingsService.GetGoalsForActiveBudget();

      int goalCount;
      goalCount = goalList.Count;
      if (goalCount == 0) {
        Console.WriteLine("Сначала создайте цель (пункт 5).");
        Console.WriteLine();
        return;
      }

      for (int goalIndex = 0; goalIndex < goalCount; goalIndex++) {
        SavingsGoal goal;
        goal = goalList[goalIndex];

        double percent;
        percent = savingsService.GetProgressPercent(goal.Id);

        string currentText;
        currentText = goal.CurrentAmount.ToString("0.00", CultureInfo.InvariantCulture);

        string targetText;
        targetText = goal.TargetAmount.ToString("0.00", CultureInfo.InvariantCulture);

        string percentText;
        percentText = percent.ToString("0.0", CultureInfo.InvariantCulture);

        Console.WriteLine(
          goal.Id
          + ") "
          + goal.Name
          + " "
          + currentText
          + "/"
          + targetText
          + " "
          + percentText
          + "%");
      }

      Console.Write("Id цели: ");
      string idLine;
      idLine = Console.ReadLine();

      int goalId;
      bool idIsNumber;
      idIsNumber = int.TryParse(idLine, out goalId);
      if (!idIsNumber) {
        Console.WriteLine("Неверный Id.");
        Console.WriteLine();
        return;
      }

      Console.Write("Сумма: ");
      string amountLine;
      amountLine = Console.ReadLine();

      double amount;
      bool amountIsNumber;
      amountIsNumber = double.TryParse(
        amountLine,
        NumberStyles.Any,
        CultureInfo.InvariantCulture,
        out amount);
      if (!amountIsNumber) {
        Console.WriteLine("Неверная сумма.");
        Console.WriteLine();
        return;
      }

      string errorMessage;
      errorMessage = "";

      bool isOk;
      isOk = savingsService.TryAddMoney(goalId, amount, out errorMessage);
      if (!isOk) {
        Console.WriteLine(errorMessage);
      } else {
        double progressPercent;
        progressPercent = savingsService.GetProgressPercent(goalId);

        string progressText;
        progressText = progressPercent.ToString("0.0", CultureInfo.InvariantCulture);

        string resultLine;
        resultLine = "Готово. Прогресс: " + progressText + "%";
        Console.WriteLine(resultLine);
      }
      Console.WriteLine();
    }

    private void CreateGoal() {
      Console.Write("Название: ");
      string name;
      name = Console.ReadLine();

      Console.Write("Целевая сумма: ");
      string targetLine;
      targetLine = Console.ReadLine();

      double targetAmount;
      bool targetIsNumber;
      targetIsNumber = double.TryParse(
        targetLine,
        NumberStyles.Any,
        CultureInfo.InvariantCulture,
        out targetAmount);
      if (!targetIsNumber) {
        Console.WriteLine("Неверная сумма.");
        Console.WriteLine();
        return;
      }

      Console.Write("Срок (гггг-мм-дд): ");
      string dateLine;
      dateLine = Console.ReadLine();

      DateTime deadline;
      bool dateIsValid;
      dateIsValid = DateTime.TryParse(dateLine, out deadline);
      if (!dateIsValid) {
        Console.WriteLine("Неверная дата.");
        Console.WriteLine();
        return;
      }

      string errorMessage;
      errorMessage = "";

      bool isOk;
      isOk = savingsService.TryCreateGoal(name, targetAmount, deadline, out errorMessage);
      if (!isOk) {
        Console.WriteLine(errorMessage);
      } else {
        Console.WriteLine("Цель создана.");
      }
      Console.WriteLine();
    }

    private void PrintGoalList() {
      List<SavingsGoal> goalList;
      goalList = savingsService.GetGoalsForActiveBudget();

      int goalCount;
      goalCount = goalList.Count;
      if (goalCount == 0) {
        Console.WriteLine("Целей нет.");
        Console.WriteLine();
        return;
      }

      for (int goalIndex = 0; goalIndex < goalCount; goalIndex++) {
        SavingsGoal goal;
        goal = goalList[goalIndex];

        double percent;
        percent = savingsService.GetProgressPercent(goal.Id);

        string dateText;
        dateText = goal.Deadline.ToString("yyyy-MM-dd");

        string currentText;
        currentText = goal.CurrentAmount.ToString("0.00", CultureInfo.InvariantCulture);

        string targetText;
        targetText = goal.TargetAmount.ToString("0.00", CultureInfo.InvariantCulture);

        string percentText;
        percentText = percent.ToString("0.0", CultureInfo.InvariantCulture);

        Console.WriteLine(
          goal.Id
          + ") "
          + goal.Name
          + " до "
          + dateText
          + " "
          + currentText
          + "/"
          + targetText
          + " "
          + percentText
          + "%");
      }
      Console.WriteLine();
    }
  }
}
