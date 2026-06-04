using System;
using System.Data.SQLite;
using System.IO;
using Budget_App.Data;

namespace Budget_App.Tests {
  internal static class TestDatabaseHelper {
    public static string CreateTempDatabase() {
      string fileName;
      fileName = Guid.NewGuid().ToString("N");

      string filePath;
      filePath = Path.Combine(Path.GetTempPath(), "budgetapp_test_" + fileName);

      string connectionString;
      connectionString = "Data Source=" + filePath + ";Version=3;";

      DatabaseSchemaInitializer.EnsureCreated(connectionString);
      return connectionString;
    }

    public static void InsertActiveBudget(string connectionString, string budgetName, double totalLimit) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          DateTime periodStart;
          periodStart = new DateTime(2026, 1, 1);

          DateTime periodEnd;
          periodEnd = new DateTime(2026, 12, 31);

          DateTime createdAt;
          createdAt = DateTime.Now;

          command.CommandText =
            "INSERT INTO Budgets (Name, Type, TotalLimit, PeriodStart, PeriodEnd, CreatedAt, IsActive) "
            + "VALUES (@name, @type, @limit, @start, @end, @created, 1);";
          command.Parameters.AddWithValue("@name", budgetName);
          command.Parameters.AddWithValue("@type", "Personal");
          command.Parameters.AddWithValue("@limit", totalLimit);
          command.Parameters.AddWithValue("@start", periodStart.ToString("o"));
          command.Parameters.AddWithValue("@end", periodEnd.ToString("o"));
          command.Parameters.AddWithValue("@created", createdAt.ToString("o"));
          command.ExecuteNonQuery();
        }
      }
    }
  }
}
