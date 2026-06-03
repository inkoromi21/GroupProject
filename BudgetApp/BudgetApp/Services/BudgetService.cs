using Budget_App.Data;
using Budget_App.Models;

namespace Budget_App.Services {
  /// <summary>
  /// Minimal budget service until Lead adds Factory Method creation flow.
  /// </summary>
  internal class BudgetService : IBudgetService {
    private readonly IRepository repository;

    public BudgetService(IRepository repository) {
      this.repository = repository;
    }

    /// <inheritdoc />
    public Budget GetActiveBudget() {
      Budget activeBudget = repository.GetActiveBudget();
      return activeBudget;
    }
  }
}
