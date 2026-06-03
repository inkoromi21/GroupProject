using System;

namespace Budget_App.Models {
  internal class Budget {
    public int Id;
    public string Name;
    public string Type;
    public double TotalLimit;
    public DateTime PeriodStart;
    public DateTime PeriodEnd;
    public DateTime CreatedAt;
    public bool IsActive;
  }
}
