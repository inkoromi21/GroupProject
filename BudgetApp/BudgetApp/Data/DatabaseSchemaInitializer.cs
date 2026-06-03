using System.Data.SQLite;

namespace Budget_App.Data {
  internal static class DatabaseSchemaInitializer {
    public static void EnsureCreated(string connectionString) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "CREATE TABLE IF NOT EXISTS Budgets ("
            + "Id INTEGER PRIMARY KEY AUTOINCREMENT, "
            + "Name TEXT NOT NULL, "
            + "Type TEXT NOT NULL, "
            + "TotalLimit REAL NOT NULL, "
            + "PeriodStart TEXT NOT NULL, "
            + "PeriodEnd TEXT NOT NULL, "
            + "CreatedAt TEXT NOT NULL, "
            + "IsActive INTEGER NOT NULL DEFAULT 0); "
            + "CREATE TABLE IF NOT EXISTS Expenses ("
            + "Id INTEGER PRIMARY KEY AUTOINCREMENT, "
            + "BudgetId INTEGER NOT NULL, "
            + "Amount REAL NOT NULL, "
            + "CategoryName TEXT NOT NULL, "
            + "Date TEXT NOT NULL, "
            + "Description TEXT NOT NULL, "
            + "FOREIGN KEY (BudgetId) REFERENCES Budgets(Id)); "
            + "CREATE TABLE IF NOT EXISTS SavingsGoals ("
            + "Id INTEGER PRIMARY KEY AUTOINCREMENT, "
            + "BudgetId INTEGER NOT NULL, "
            + "Name TEXT NOT NULL, "
            + "TargetAmount REAL NOT NULL, "
            + "CurrentAmount REAL NOT NULL DEFAULT 0, "
            + "Deadline TEXT NOT NULL, "
            + "FOREIGN KEY (BudgetId) REFERENCES Budgets(Id));";
          command.ExecuteNonQuery();
        }
      }
    }
  }
}
