using Budget_App.Models;

namespace Budget_App.Services {
  internal interface IBudgetService {
    Budget GetActiveBudget();
  }
}
