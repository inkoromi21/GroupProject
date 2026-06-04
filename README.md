# BudgetApp — приложение для составления бюджета

## Аннотация

Консольное приложение (тема #10): создание бюджета, учёт расходов, цели сбережений, отчёты.  
Архитектура **MVC**, база **SQLite**. Паттерны: **Factory Method**, **Strategy**, **Observer**.

## Участники

| Участник | Папка | Паттерн | Ветка (PR → main) |
|----------|-------|---------|-------------------|
| Воробьев Семен Евгеньевич — Lead | `Stores/`, `Budgets/` | Factory Method | `dev_budget-creation` |
| Дорошкевич Матвей Евгеньевич | `Reports/` | Strategy | `dev_expense-reports` |
| Солдатов Владимир Сергеевич | `Observers/` | Observer | `dev_savings-observer` |

## Структура проекта

```
BudgetApp/
  BudgetApp/
    Program.cs
    ConsoleUI/
    Models/
    Data/
    Stores/
    Budgets/
    Reports/
    Observers/
    Controllers/
    Services/
    AppConstants/
    Properties/
  BudgetApp.Tests/
```

## Меню

| Пункт | Действие |
|-------|----------|
| 1 | Создать бюджет |
| 2 | Выбрать активный бюджет |
| 3–4 | Расходы (добавить / список) |
| 5–6 | Сбережения (цели / пополнение) |
| 7 | Отчёт по бюджету |
| 0 | Выход |

---

## Участник 1 — Lead (Воробьев)

**Factory Method**

- `BudgetStore` — абстрактный создатель, `CreateTemplate(BudgetType)`
- `SimpleBudgetStore` — конкретная реализация, выбор шаблона по типу
- `Budgets/` — `BudgetTemplate`, `PersonalBudgetTemplate`, `FamilyBudgetTemplate`, `BusinessBudgetTemplate`

**MVC**

- `BudgetController` — пункты меню 1 и 2
- `BudgetService` — создание бюджета, список, выбор активного; `IBudgetService.GetActiveBudget()`
- `BudgetRepository` — таблица `Budgets` в SQLite

**Тесты:** `BudgetStoreTests`, `BudgetServiceTests`

**Ветка:** `dev_budget-creation`

---

## Участник 2 — Дорошкевич

**Strategy**

- `IReportStrategy` — общий интерфейс отчёта, `Generate(ReportData data)`
- `SummaryReportStrategy`, `MonthlyReportStrategy`, `ByCategoryReportStrategy` — варианты отчёта
- `ReportContext` — выбор и запуск стратегии

**MVC**

- `ExpenseController` — пункты меню 3 и 4
- `ExpenseService` — добавление и список расходов по активному бюджету
- `ReportController` — пункт меню 7, подменю типа отчёта
- `SqliteRepository` (`IRepository`) — расходы и данные для отчётов

**Тесты:** `ExpenseServiceTests`, `SummaryReportStrategyTests`, `MonthlyReportStrategyTests`, `ByCategoryReportStrategyTests`

**Ветка:** `dev_expense-reports`

---

## Участник 3 — Солдатов

**Observer**

- `IBudgetSubject`, `BudgetSubject` — субъект: `Attach`, `Detach`, `Notify`
- `IBudgetObserver`, `ConsoleBudgetObserver` — подписчик, вывод `[Уведомление]` в консоль
- `BudgetEventArgs`, `BudgetEventType` — тип события и данные уведомления
- `Notify` вызывается из `BudgetService`, `ExpenseService`, `SavingsService` при создании бюджета, расходах, целях и пополнении

**MVC (сбережения)**

- `SavingsController` — пункты меню 5 и 6 (подменю целей, пополнение)
- `SavingsService` — создание цели, список, пополнение
- `SavingsRepository` — таблица `SavingsGoals`, активный бюджет

**Тесты:** `BudgetSubjectTests`, `SavingsServiceTests`

**Ветка:** `dev_savings-observer`
