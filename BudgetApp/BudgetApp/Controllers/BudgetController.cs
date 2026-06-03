using System;
using System.Collections.Generic;
using System.Globalization;
using Budget_App.AppConstants;
using Budget_App.Models;
using Budget_App.Services;

namespace Budget_App.Controllers {
  internal class BudgetController {
    private readonly BudgetService budgetService;

    public BudgetController(BudgetService budgetService) {
      this.budgetService = budgetService;
    }

    public void CreateBudget() {
      Console.Write("Название бюджета: ");
      string name = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(name)) {
        Console.WriteLine("Название обязательно.");
        return;
      }

      Console.WriteLine("Тип: 1 — Personal, 2 — Family, 3 — Project");
      Console.Write("Тип: ");
      string typeLine = Console.ReadLine();
      int budgetTypeCode = 0;
      if (!int.TryParse(typeLine, out budgetTypeCode)) {
        Console.WriteLine("Неверный тип.");
        return;
      }

      Console.Write("Лимит: ");
      string limitLine = Console.ReadLine();
      double totalLimit = 0.0;
      if (!double.TryParse(limitLine, NumberStyles.Any, CultureInfo.InvariantCulture, out totalLimit)) {
        Console.WriteLine("Неверный лимит.");
        return;
      }

      if (totalLimit <= 0.0) {
        Console.WriteLine("Лимит должен быть больше нуля.");
        return;
      }

      DateTime periodStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      DateTime periodEnd = periodStart.AddMonths(1).AddDays(-1.0);

      budgetService.Create(name.Trim(), budgetTypeCode, totalLimit, periodStart, periodEnd);
      Console.WriteLine("Бюджет создан.");
    }

    public void SelectActiveBudget() {
      List<Budget> budgetList = budgetService.GetAll();
      if (budgetList.Count == 0) {
        Console.WriteLine("Нет бюджетов. Сначала создайте бюджет (пункт 1).");
        return;
      }

      int budgetCount = budgetList.Count;
      Console.WriteLine("--- Список бюджетов ---");
      for (int budgetIndex = 0; budgetIndex < budgetCount; budgetIndex++) {
        Budget budget = budgetList[budgetIndex];
        string activeMark = "";
        if (budget.IsActive) {
          activeMark = " [активный]";
        }
        Console.WriteLine(
          budget.Id
          + ") "
          + budget.Name
          + " | "
          + budget.Type
          + " | "
          + budget.TotalLimit.ToString("0.00", CultureInfo.InvariantCulture)
          + activeMark);
      }

      Console.Write("Id активного бюджета: ");
      string idLine = Console.ReadLine();
      int budgetId = 0;
      if (!int.TryParse(idLine, out budgetId)) {
        Console.WriteLine("Неверный id.");
        return;
      }

      Budget found = null;
      for (int budgetIndex = 0; budgetIndex < budgetCount; budgetIndex++) {
        if (budgetList[budgetIndex].Id == budgetId) {
          found = budgetList[budgetIndex];
          break;
        }
      }

      if (found == null) {
        Console.WriteLine("Бюджет не найден.");
        return;
      }

      budgetService.SetActive(budgetId);
      Console.WriteLine("Активный бюджет выбран.");
    }
  }
}
