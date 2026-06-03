using Budget_App.Observers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget_App.Tests.Observers {
  [TestClass]
  public class BudgetSubjectTests {
    private class TestObserver : IBudgetObserver {
      public int updateCount;

      public void Update(BudgetEventArgs eventArgs) {
        updateCount = updateCount + 1;
      }
    }

    [TestMethod]
    public void Notify_AfterAttach_CallsUpdateOnce() {
      BudgetSubject subject = new BudgetSubject();
      TestObserver observer = new TestObserver();
      subject.Attach(observer);

      BudgetEventArgs eventArgs = new BudgetEventArgs();
      eventArgs.message = "test";
      eventArgs.eventType = BudgetEventType.BudgetCreated;
      subject.Notify(eventArgs);

      Assert.AreEqual(1, observer.updateCount);
    }

    [TestMethod]
    public void Notify_AfterDetach_DoesNotCallUpdate() {
      BudgetSubject subject = new BudgetSubject();
      TestObserver observer = new TestObserver();
      subject.Attach(observer);
      subject.Detach(observer);

      BudgetEventArgs eventArgs = new BudgetEventArgs();
      eventArgs.message = "test";
      subject.Notify(eventArgs);

      Assert.AreEqual(0, observer.updateCount);
    }
  }
}
