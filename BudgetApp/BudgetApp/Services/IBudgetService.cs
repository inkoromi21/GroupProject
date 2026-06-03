using Budget_App.Models;

namespace Budget_App.Services {
  /// <summary>
  /// Budget operations used by other modules (Lead extends this later).
  /// </summary>
  internal interface IBudgetService {
    Budget GetActiveBudget();
  }
}
