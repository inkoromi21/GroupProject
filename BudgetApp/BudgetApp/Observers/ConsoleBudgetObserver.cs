using System;

namespace Budget_App.Observers {
  internal class ConsoleBudgetObserver : IBudgetObserver {
    public void Update(BudgetEventArgs eventArgs) {
      if (eventArgs == null) {
        return;
      }

      bool messageIsEmpty;
      messageIsEmpty = string.IsNullOrEmpty(eventArgs.message);
      if (messageIsEmpty) {
        return;
      }

      string notificationPrefix;
      notificationPrefix = "[Уведомление] ";

      string fullMessage;
      fullMessage = notificationPrefix + eventArgs.message;

      Console.WriteLine(fullMessage);
    }
  }
}
