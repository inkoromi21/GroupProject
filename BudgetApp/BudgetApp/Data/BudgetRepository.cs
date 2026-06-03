using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Budget_App.Models;

namespace Budget_App.Data {
  internal class BudgetRepository {
    private readonly string connectionString;

    public BudgetRepository(string connectionString) {
      this.connectionString = connectionString;
    }

    public int Add(Budget budget) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "INSERT INTO Budgets (Name, Type, TotalLimit, PeriodStart, PeriodEnd, CreatedAt, IsActive) "
            + "VALUES ($name, $type, $limit, $start, $end, $created, $active); "
            + "SELECT last_insert_rowid();";
          command.Parameters.AddWithValue("$name", budget.Name);
          command.Parameters.AddWithValue("$type", budget.Type);
          command.Parameters.AddWithValue("$limit", budget.TotalLimit);
          command.Parameters.AddWithValue("$start", budget.PeriodStart.ToString("O"));
          command.Parameters.AddWithValue("$end", budget.PeriodEnd.ToString("O"));
          command.Parameters.AddWithValue("$created", budget.CreatedAt.ToString("O"));
          int activeFlag = 0;
          if (budget.IsActive) {
            activeFlag = 1;
          }
          command.Parameters.AddWithValue("$active", activeFlag);
          object result = command.ExecuteScalar();
          return Convert.ToInt32(result);
        }
      }
    }

    public List<Budget> GetAll() {
      List<Budget> budgets = new List<Budget>();
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, Name, Type, TotalLimit, PeriodStart, PeriodEnd, CreatedAt, IsActive "
            + "FROM Budgets ORDER BY Id;";
          using (SQLiteDataReader reader = command.ExecuteReader()) {
            while (reader.Read()) {
              budgets.Add(ReadBudget(reader));
            }
          }
        }
      }
      return budgets;
    }

    public Budget GetById(int budgetId) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, Name, Type, TotalLimit, PeriodStart, PeriodEnd, CreatedAt, IsActive "
            + "FROM Budgets WHERE Id = $id;";
          command.Parameters.AddWithValue("$id", budgetId);
          using (SQLiteDataReader reader = command.ExecuteReader()) {
            if (!reader.Read()) {
              return null;
            }
            return ReadBudget(reader);
          }
        }
      }
    }

    public void SetActive(int budgetId) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand clearCommand = connection.CreateCommand()) {
          clearCommand.CommandText = "UPDATE Budgets SET IsActive = 0;";
          clearCommand.ExecuteNonQuery();
        }
        using (SQLiteCommand setCommand = connection.CreateCommand()) {
          setCommand.CommandText = "UPDATE Budgets SET IsActive = 1 WHERE Id = $id;";
          setCommand.Parameters.AddWithValue("$id", budgetId);
          setCommand.ExecuteNonQuery();
        }
      }
    }

    public Budget GetActive() {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, Name, Type, TotalLimit, PeriodStart, PeriodEnd, CreatedAt, IsActive "
            + "FROM Budgets WHERE IsActive = 1 LIMIT 1;";
          using (SQLiteDataReader reader = command.ExecuteReader()) {
            if (!reader.Read()) {
              return null;
            }
            return ReadBudget(reader);
          }
        }
      }
    }

    private static Budget ReadBudget(SQLiteDataReader reader) {
      Budget budget = new Budget();
      budget.Id = reader.GetInt32(0);
      budget.Name = reader.GetString(1);
      budget.Type = reader.GetString(2);
      budget.TotalLimit = reader.GetDouble(3);
      budget.PeriodStart = DateTime.Parse(reader.GetString(4));
      budget.PeriodEnd = DateTime.Parse(reader.GetString(5));
      budget.CreatedAt = DateTime.Parse(reader.GetString(6));
      budget.IsActive = reader.GetInt32(7) == 1;
      return budget;
    }
  }
}
