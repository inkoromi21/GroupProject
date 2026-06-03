using System;
using System.Collections.Generic;
using System.Globalization;
using Budget_App.Models;
using Budget_App.Services;

namespace Budget_App.Controllers {
  internal class ExpenseController {
    private readonly IExpenseService expenseService;

    public ExpenseController(IExpenseService expenseService) {
      this.expenseService = expenseService;
    }

    public void AddExpense() {
      Console.Write("Сумма: ");
      string amountLine = Console.ReadLine();
      double amount = 0.0;
      if (!double.TryParse(amountLine, NumberStyles.Any, CultureInfo.InvariantCulture, out amount)) {
        Console.WriteLine("Неверная сумма.");
        return;
      }

      Console.Write("Категория: ");
      string categoryName = Console.ReadLine();
      Console.Write("Описание: ");
      string description = Console.ReadLine();

      string errorMessage = "";
      bool saved = expenseService.TryAddExpense(amount, categoryName, description, out errorMessage);
      if (!saved) {
        Console.WriteLine(errorMessage);
        return;
      }
      Console.WriteLine("Расход сохранён.");
    }

    public void ListExpenses() {
      List<Expense> expenseList = expenseService.GetExpensesForActiveBudget();
      if (expenseList.Count == 0) {
        Console.WriteLine("Нет расходов по активному бюджету.");
        return;
      }

      double listTotal = 0.0;
      int expenseCount = expenseList.Count;
      Console.WriteLine("--- Список расходов ---");
      for (int expenseIndex = 0; expenseIndex < expenseCount; expenseIndex++) {
        Expense expense = expenseList[expenseIndex];
        string amountText = expense.Amount.ToString("0.00", CultureInfo.InvariantCulture);
        Console.WriteLine(
          expense.Id
          + ") "
          + expense.Date.ToString("yyyy-MM-dd")
          + " | "
          + expense.CategoryName
          + " | "
          + amountText
          + " | "
          + expense.Description);
        listTotal = listTotal + expense.Amount;
      }
      string totalText = listTotal.ToString("0.00", CultureInfo.InvariantCulture);
      Console.WriteLine("Итого: " + totalText);

      Console.Write("Удалить по id (Enter — пропустить): ");
      string idLine = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(idLine)) {
        return;
      }

      int expenseId = 0;
      if (!int.TryParse(idLine, out expenseId)) {
        Console.WriteLine("Неверный id.");
        return;
      }

      string deleteError = "";
      bool deleted = expenseService.TryDeleteExpense(expenseId, out deleteError);
      if (!deleted) {
        Console.WriteLine(deleteError);
        return;
      }
      Console.WriteLine("Расход удалён.");
    }
  }
}
