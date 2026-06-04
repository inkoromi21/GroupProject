using System;
using System.Collections.Generic;
using System.Globalization;
using Budget_App.AppConstants;
using Budget_App.ConsoleUI;
using Budget_App.Models;
using Budget_App.Services;

namespace Budget_App.Controllers {
  internal class BudgetController {
    private readonly BudgetService budgetService;

    public BudgetController(BudgetService budgetService) {
      this.budgetService = budgetService;
    }

    public void CreateBudget() {
      Console.WriteLine("Тип бюджета: 1 — Personal, 2 — Family, 3 — Business");
      BudgetType budgetType = ConsoleMenu.ReadBudgetType();
      if (budgetType == BudgetType.Unknown) {
        Console.WriteLine("Неверный тип.");
        Console.WriteLine();
        return;
      }

      Console.WriteLine("Название (Enter — имя по умолчанию):");
      string name = Console.ReadLine();
      if (name == null) {
        name = "";
      }

      Console.WriteLine("Лимит (Enter — лимит по умолчанию):");
      string limitLine = Console.ReadLine();
      double customLimit = BudgetConstants.NoCustomLimit;
      if (limitLine != null && limitLine.Trim().Length > 0) {
        double parsedLimit;
        bool isParsed = double.TryParse(
          limitLine.Trim(),
          NumberStyles.Any,
          CultureInfo.InvariantCulture,
          out parsedLimit);
        if (!isParsed) {
          Console.WriteLine("Неверный лимит.");
          Console.WriteLine();
          return;
        }
        customLimit = parsedLimit;
      }

      Budget created = budgetService.Create(name, budgetType, customLimit);
      if (created == null) {
        Console.WriteLine("Не удалось создать бюджет. Проверьте лимит.");
      } else {
        Console.WriteLine("Бюджет создан: Id=" + created.Id + ", " + created.Name
          + ", лимит " + created.TotalLimit + ".");
      }
      Console.WriteLine();
    }

    public void SelectActiveBudget() {
      List<Budget> budgets = budgetService.GetAll();
      if (budgets.Count == 0) {
        Console.WriteLine("Бюджетов нет. Создайте бюджет (п.1).");
        Console.WriteLine();
        return;
      }

      Console.WriteLine("Список бюджетов:");
      for (int budgetIndex = 0; budgetIndex < budgets.Count; budgetIndex++) {
        Budget item = budgets[budgetIndex];
        string activeMark = "";
        if (item.IsActive) {
          activeMark = " [активный]";
        }
        Console.WriteLine("  " + item.Id + ". " + item.Name + " (" + item.Type + ") лимит "
          + item.TotalLimit + activeMark);
      }

      Console.WriteLine("Введите Id активного бюджета:");
      string idLine = Console.ReadLine();
      int budgetId;
      bool isIdParsed = int.TryParse(idLine, out budgetId);
      if (!isIdParsed) {
        Console.WriteLine("Неверный Id.");
        Console.WriteLine();
        return;
      }

      bool isSet = budgetService.SetActive(budgetId);
      if (!isSet) {
        Console.WriteLine("Бюджет не найден.");
      } else {
        Console.WriteLine("Активный бюджет: Id=" + budgetId + ".");
      }
      Console.WriteLine();
    }
  }
}
