using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Budget_App.Models;

namespace Budget_App.Data {
  internal class SqliteRepository : IRepository {
    private readonly string connectionString;

    public SqliteRepository(string connectionString) {
      this.connectionString = connectionString;
    }

    public Budget GetActiveBudget() {
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
            return ReadBudgetFromReader(reader);
          }
        }
      }
    }

    public Budget GetBudgetById(int budgetId) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, Name, Type, TotalLimit, PeriodStart, PeriodEnd, CreatedAt, IsActive "
            + "FROM Budgets WHERE Id = @id;";
          command.Parameters.AddWithValue("@id", budgetId);
          using (SQLiteDataReader reader = command.ExecuteReader()) {
            if (!reader.Read()) {
              return null;
            }
            return ReadBudgetFromReader(reader);
          }
        }
      }
    }

    public List<Budget> GetAllBudgets() {
      List<Budget> budgetList = new List<Budget>();
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, Name, Type, TotalLimit, PeriodStart, PeriodEnd, CreatedAt, IsActive "
            + "FROM Budgets ORDER BY Id;";
          using (SQLiteDataReader reader = command.ExecuteReader()) {
            while (reader.Read()) {
              Budget budget = ReadBudgetFromReader(reader);
              budgetList.Add(budget);
            }
          }
        }
      }
      return budgetList;
    }

    public int AddExpense(Expense expense) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "INSERT INTO Expenses (BudgetId, Amount, CategoryName, Date, Description) "
            + "VALUES (@budgetId, @amount, @categoryName, @date, @description); "
            + "SELECT last_insert_rowid();";
          command.Parameters.AddWithValue("@budgetId", expense.BudgetId);
          command.Parameters.AddWithValue("@amount", expense.Amount);
          command.Parameters.AddWithValue("@categoryName", expense.CategoryName);
          command.Parameters.AddWithValue("@date", expense.Date.ToString("o"));
          command.Parameters.AddWithValue("@description", expense.Description);
          object result = command.ExecuteScalar();
          return Convert.ToInt32(result);
        }
      }
    }

    public List<Expense> GetExpensesByBudgetId(int budgetId) {
      List<Expense> expenseList = new List<Expense>();
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, BudgetId, Amount, CategoryName, Date, Description "
            + "FROM Expenses WHERE BudgetId = @budgetId ORDER BY Date DESC, Id DESC;";
          command.Parameters.AddWithValue("@budgetId", budgetId);
          using (SQLiteDataReader reader = command.ExecuteReader()) {
            while (reader.Read()) {
              Expense expense = ReadExpenseFromReader(reader);
              expenseList.Add(expense);
            }
          }
        }
      }
      return expenseList;
    }

    public Expense GetExpenseById(int expenseId) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, BudgetId, Amount, CategoryName, Date, Description "
            + "FROM Expenses WHERE Id = @id;";
          command.Parameters.AddWithValue("@id", expenseId);
          using (SQLiteDataReader reader = command.ExecuteReader()) {
            if (!reader.Read()) {
              return null;
            }
            return ReadExpenseFromReader(reader);
          }
        }
      }
    }

    public bool DeleteExpense(int expenseId) {
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText = "DELETE FROM Expenses WHERE Id = @id;";
          command.Parameters.AddWithValue("@id", expenseId);
          int rowsAffected = command.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public List<SavingsGoal> GetSavingsGoalsByBudgetId(int budgetId) {
      List<SavingsGoal> goalList = new List<SavingsGoal>();
      using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
        connection.Open();
        using (SQLiteCommand command = connection.CreateCommand()) {
          command.CommandText =
            "SELECT Id, BudgetId, Name, TargetAmount, CurrentAmount, Deadline "
            + "FROM SavingsGoals WHERE BudgetId = @budgetId ORDER BY Id;";
          command.Parameters.AddWithValue("@budgetId", budgetId);
          using (SQLiteDataReader reader = command.ExecuteReader()) {
            while (reader.Read()) {
              SavingsGoal goal = ReadSavingsGoalFromReader(reader);
              goalList.Add(goal);
            }
          }
        }
      }
      return goalList;
    }

    private static Budget ReadBudgetFromReader(SQLiteDataReader reader) {
      Budget budget = new Budget();
      budget.Id = reader.GetInt32(0);
      budget.Name = reader.GetString(1);
      budget.Type = reader.GetString(2);
      budget.TotalLimit = reader.GetDouble(3);
      budget.PeriodStart = DateTime.Parse(reader.GetString(4));
      budget.PeriodEnd = DateTime.Parse(reader.GetString(5));
      budget.CreatedAt = DateTime.Parse(reader.GetString(6));
      int activeFlag = reader.GetInt32(7);
      budget.IsActive = activeFlag != 0;
      return budget;
    }

    private static Expense ReadExpenseFromReader(SQLiteDataReader reader) {
      Expense expense = new Expense();
      expense.Id = reader.GetInt32(0);
      expense.BudgetId = reader.GetInt32(1);
      expense.Amount = reader.GetDouble(2);
      expense.CategoryName = reader.GetString(3);
      expense.Date = DateTime.Parse(reader.GetString(4));
      expense.Description = reader.GetString(5);
      return expense;
    }

    private static SavingsGoal ReadSavingsGoalFromReader(SQLiteDataReader reader) {
      SavingsGoal goal = new SavingsGoal();
      goal.Id = reader.GetInt32(0);
      goal.BudgetId = reader.GetInt32(1);
      goal.Name = reader.GetString(2);
      goal.TargetAmount = reader.GetDouble(3);
      goal.CurrentAmount = reader.GetDouble(4);
      goal.Deadline = DateTime.Parse(reader.GetString(5));
      return goal;
    }
  }
}
