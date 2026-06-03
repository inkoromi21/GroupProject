using System.Collections.Generic;

namespace Budget_App.Observers {
  public class BudgetSubject : IBudgetSubject {
    private readonly List<IBudgetObserver> observerList;

    public BudgetSubject() {
      observerList = new List<IBudgetObserver>();
    }

    public void Attach(IBudgetObserver observer) {
      if (observer == null) {
        return;
      }

      bool observerAlreadyAttached;
      observerAlreadyAttached = observerList.Contains(observer);
      if (!observerAlreadyAttached) {
        observerList.Add(observer);
      }
    }

    public void Detach(IBudgetObserver observer) {
      if (observer == null) {
        return;
      }

      observerList.Remove(observer);
    }

    public void Notify(BudgetEventArgs eventArgs) {
      if (eventArgs == null) {
        return;
      }

      int observerCount;
      observerCount = observerList.Count;

      for (int observerIndex = 0; observerIndex < observerCount; observerIndex++) {
        IBudgetObserver currentObserver;
        currentObserver = observerList[observerIndex];
        currentObserver.Update(eventArgs);
      }
    }
  }
}
