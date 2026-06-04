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
      BudgetSubject subject;
      subject = new BudgetSubject();

      TestObserver observer;
      observer = new TestObserver();

      subject.Attach(observer);

      BudgetEventArgs eventArgs;
      eventArgs = new BudgetEventArgs();
      eventArgs.message = "test";
      eventArgs.eventType = BudgetEventType.BudgetCreated;

      subject.Notify(eventArgs);

      int expectedCount;
      expectedCount = 1;

      Assert.AreEqual(expectedCount, observer.updateCount);
    }

    [TestMethod]
    public void Notify_AfterDetach_DoesNotCallUpdate() {
      BudgetSubject subject;
      subject = new BudgetSubject();

      TestObserver observer;
      observer = new TestObserver();

      subject.Attach(observer);
      subject.Detach(observer);

      BudgetEventArgs eventArgs;
      eventArgs = new BudgetEventArgs();
      eventArgs.message = "test";

      subject.Notify(eventArgs);

      int expectedCount;
      expectedCount = 0;

      Assert.AreEqual(expectedCount, observer.updateCount);
    }
  }
}
