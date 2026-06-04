using System;

namespace Budget_App.ConsoleUI {
  internal static class ConsoleMenu {
    public static string ReadAction() {
      Console.Clear();
      Console.WriteLine("=== BUDGET APP ===");
      Console.WriteLine("Выберите действие:");
      Console.WriteLine("1) Создать бюджет");
      Console.WriteLine("2) Выбрать активный бюджет");
      Console.WriteLine("3) Добавить расход");
      Console.WriteLine("4) Список расходов");
      Console.WriteLine("5) Цели сбережений");
      Console.WriteLine("6) Пополнить сбережения");
      Console.WriteLine("7) Бюджетный отчёт");
      Console.WriteLine("0) Выход");
      Console.Write("Ваш выбор: ");
      string line = Console.ReadLine();
      if (line == null) {
        return "";
      }
      return line.Trim();
    }

    public static BudgetType ReadBudgetType() {
      Console.Write("Ваш выбор: ");
      string line = Console.ReadLine();
      if (line == null) {
        return BudgetType.Unknown;
      }
      line = line.Trim();
      if (line == "1") {
        return BudgetType.Personal;
      }
      if (line == "2") {
        return BudgetType.Family;
      }
      if (line == "3") {
        return BudgetType.Business;
      }
      return BudgetType.Unknown;
    }
  }
}
