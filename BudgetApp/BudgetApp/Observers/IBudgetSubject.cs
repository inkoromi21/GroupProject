namespace Budget_App.Observers {
  public interface IBudgetSubject {
    void Attach(IBudgetObserver observer);

    void Detach(IBudgetObserver observer);

    void Notify(BudgetEventArgs eventArgs);
  }
}
