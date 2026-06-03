using System;
using System.Data.SQLite;
using System.IO;
using Budget_App.Data;

namespace Budget_App.Tests {
  internal static class TestDatabaseHelper {
    public static string CreateTempDatabase() {
      string filePath = Path.Combine(Path.GetTempPath(), "budgetapp_test_" + Guid.NewGuid().ToString("N") + ".db");
      string connectionString = "Data Source=" + filePath + ";Version=3;";
      DatabaseSchemaInitializer.EnsureCreated(connectionString);
      return connectionString;
    }
  }
}
