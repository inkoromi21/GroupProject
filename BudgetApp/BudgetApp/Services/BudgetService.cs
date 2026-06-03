using Budget_App.Data;
using Budget_App.Models;

namespace Budget_App.Services {
  internal class BudgetService : IBudgetService {
    private readonly IRepository repository;

    public BudgetService(IRepository repository) {
      this.repository = repository;
    }

    public Budget GetActiveBudget() {
      Budget activeBudget = repository.GetActiveBudget();
      return activeBudget;
    }
  }
}
