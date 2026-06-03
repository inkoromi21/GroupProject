using System;
using Budget_App.Services;

namespace Budget_App.Controllers {
  /// <summary>
  /// Console flow for expense menu items.
  /// </summary>
  internal class ExpenseController {
    private readonly IExpenseService expenseService;

    public ExpenseController(IExpenseService expenseService) {
      this.expenseService = expenseService;
    }

    /// <summary>
    /// Menu item 3: add expense.
    /// </summary>
    public void AddExpense() {
      Console.Write("Amount: ");
      string amountLine = Console.ReadLine();
      double amount = 0.0;
      if (!double.TryParse(amountLine, out amount)) {
        Console.WriteLine("Invalid amount.");
        return;
      }

      Console.Write("Category: ");
      string categoryName = Console.ReadLine();
      Console.Write("Description: ");
      string description = Console.ReadLine();

      string errorMessage = "";
      bool saved = expenseService.TryAddExpense(amount, categoryName, description, out errorMessage);
      if (!saved) {
        Console.WriteLine(errorMessage);
        return;
      }
      Console.WriteLine("Expense saved.");
    }

    /// <summary>
    /// Menu item 4: list expenses and optional delete.
    /// </summary>
    public void ListExpenses() {
      System.Collections.Generic.List<Budget_App.Models.Expense> expenseList =
        expenseService.GetExpensesForActiveBudget();
      if (expenseList.Count == 0) {
        Console.WriteLine("No expenses for the active budget.");
        return;
      }

      double listTotal = 0.0;
      int expenseCount = expenseList.Count;
      Console.WriteLine("--- Expense list ---");
      for (int expenseIndex = 0; expenseIndex < expenseCount; expenseIndex++) {
        Budget_App.Models.Expense expense = expenseList[expenseIndex];
        Console.WriteLine(
          expense.Id
          + ") "
          + expense.Date.ToString("yyyy-MM-dd")
          + " | "
          + expense.CategoryName
          + " | "
          + expense.Amount.ToString("0.00")
          + " | "
          + expense.Description);
        listTotal = listTotal + expense.Amount;
      }
      Console.WriteLine("Total: " + listTotal.ToString("0.00"));

      Console.Write("Delete by id (Enter to skip): ");
      string idLine = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(idLine)) {
        return;
      }

      int expenseId = 0;
      if (!int.TryParse(idLine, out expenseId)) {
        Console.WriteLine("Invalid id.");
        return;
      }

      string deleteError = "";
      bool deleted = expenseService.TryDeleteExpense(expenseId, out deleteError);
      if (!deleted) {
        Console.WriteLine(deleteError);
        return;
      }
      Console.WriteLine("Expense deleted.");
    }
  }
}
