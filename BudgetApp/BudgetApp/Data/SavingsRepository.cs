using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Budget_App.Models;

namespace Budget_App.Data {
  internal class SavingsRepository {
    private readonly string connectionString;

    public SavingsRepository(string connectionString) {
      this.connectionString = connectionString;
    }

    public int Add(SavingsGoal goal) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "INSERT INTO SavingsGoals (BudgetId, Name, TargetAmount, CurrentAmount, Deadline) "
            + "VALUES ($budgetId, $name, $target, $current, $deadline); "
            + "SELECT last_insert_rowid();";
          command.Parameters.AddWithValue("$budgetId", goal.BudgetId);
          command.Parameters.AddWithValue("$name", goal.Name);
          command.Parameters.AddWithValue("$target", goal.TargetAmount);
          command.Parameters.AddWithValue("$current", goal.CurrentAmount);
          command.Parameters.AddWithValue("$deadline", goal.Deadline.ToString("o"));

          object insertResult;
          insertResult = command.ExecuteScalar();

          int newId;
          newId = Convert.ToInt32(insertResult);
          return newId;
        }
      }
    }

    public List<SavingsGoal> GetByBudgetId(int budgetId) {
      List<SavingsGoal> goalList;
      goalList = new List<SavingsGoal>();

      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, BudgetId, Name, TargetAmount, CurrentAmount, Deadline "
            + "FROM SavingsGoals WHERE BudgetId = $budgetId ORDER BY Id;";
          command.Parameters.AddWithValue("$budgetId", budgetId);

          using (SQLiteDataReader reader = command.ExecuteReader()) {
            while (reader.Read()) {
              SavingsGoal goal;
              goal = ReadGoal(reader);
              goalList.Add(goal);
            }
          }
        }
      }

      return goalList;
    }

    public SavingsGoal GetById(int goalId) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, BudgetId, Name, TargetAmount, CurrentAmount, Deadline "
            + "FROM SavingsGoals WHERE Id = $id;";
          command.Parameters.AddWithValue("$id", goalId);

          using (SQLiteDataReader reader = command.ExecuteReader()) {
            bool rowExists;
            rowExists = reader.Read();
            if (!rowExists) {
              return null;
            }

            SavingsGoal goal;
            goal = ReadGoal(reader);
            return goal;
          }
        }
      }
    }

    public bool UpdateCurrentAmount(int goalId, double newAmount) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText = "UPDATE SavingsGoals SET CurrentAmount = $current WHERE Id = $id;";
          command.Parameters.AddWithValue("$current", newAmount);
          command.Parameters.AddWithValue("$id", goalId);

          int rowCount;
          rowCount = command.ExecuteNonQuery();

          bool isUpdated;
          isUpdated = rowCount > 0;
          return isUpdated;
        }
      }
    }

    public Budget GetActiveBudget() {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, Name, Type, TotalLimit, PeriodStart, PeriodEnd, CreatedAt, IsActive "
            + "FROM Budgets WHERE IsActive = 1 LIMIT 1;";

          using (SQLiteDataReader reader = command.ExecuteReader()) {
            bool rowExists;
            rowExists = reader.Read();
            if (!rowExists) {
              return null;
            }

            Budget budget;
            budget = ReadBudget(reader);
            return budget;
          }
        }
      }
    }

    private static SavingsGoal ReadGoal(SQLiteDataReader reader) {
      int columnId;
      columnId = 0;

      int columnBudgetId;
      columnBudgetId = 1;

      int columnName;
      columnName = 2;

      int columnTargetAmount;
      columnTargetAmount = 3;

      int columnCurrentAmount;
      columnCurrentAmount = 4;

      int columnDeadline;
      columnDeadline = 5;

      SavingsGoal goal;
      goal = new SavingsGoal();
      goal.Id = reader.GetInt32(columnId);
      goal.BudgetId = reader.GetInt32(columnBudgetId);
      goal.Name = reader.GetString(columnName);
      goal.TargetAmount = reader.GetDouble(columnTargetAmount);
      goal.CurrentAmount = reader.GetDouble(columnCurrentAmount);
      goal.Deadline = DateTime.Parse(reader.GetString(columnDeadline));
      return goal;
    }

    private static Budget ReadBudget(SQLiteDataReader reader) {
      int columnId;
      columnId = 0;

      int columnName;
      columnName = 1;

      int columnType;
      columnType = 2;

      int columnTotalLimit;
      columnTotalLimit = 3;

      int columnPeriodStart;
      columnPeriodStart = 4;

      int columnPeriodEnd;
      columnPeriodEnd = 5;

      int columnCreatedAt;
      columnCreatedAt = 6;

      int columnIsActive;
      columnIsActive = 7;

      int activeFlagValue;
      activeFlagValue = 1;

      Budget budget;
      budget = new Budget();
      budget.Id = reader.GetInt32(columnId);
      budget.Name = reader.GetString(columnName);
      budget.Type = reader.GetString(columnType);
      budget.TotalLimit = reader.GetDouble(columnTotalLimit);
      budget.PeriodStart = DateTime.Parse(reader.GetString(columnPeriodStart));
      budget.PeriodEnd = DateTime.Parse(reader.GetString(columnPeriodEnd));
      budget.CreatedAt = DateTime.Parse(reader.GetString(columnCreatedAt));

      int activeFlag;
      activeFlag = reader.GetInt32(columnIsActive);
      if (activeFlag == activeFlagValue) {
        budget.IsActive = true;
      } else {
        budget.IsActive = false;
      }

      return budget;
    }
  }
}
