# BudgetApp — приложение для составления бюджета

## Аннотация

Консольное приложение для учёта бюджета: создание бюджета, расходы, сбережения, отчёты. Архитектура MVC, база SQLite.

## Участники

| Участник | Папка | Паттерн |
|----------|-------|---------|
| Воробьев Семен Евгеньевич | `Stores/`, `Budgets/` | Factory Method |
| Дорошкевич Матвей Евгеньевич | `Reports/` | Strategy |
| Солдатов Владимир Сергеевич | `Observers/` | Observer |

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

1 — создать бюджет · 2 — выбрать активный бюджет · 3–4 — расходы · 5–6 — сбережения · 7 — отчёт · 0 — выход

## Участник 1 — Lead (Воробьев)

**Factory Method**

- `BudgetStore` — абстрактный создатель, `CreateTemplate(BudgetType)`
- `SimpleBudgetStore` — конкретная реализация, выбор шаблона по типу
- `Budgets/` — `PersonalBudgetTemplate`, `FamilyBudgetTemplate`, `BusinessBudgetTemplate`

**MVC**

- `BudgetController` — пункты меню 1 и 2
- `BudgetService` — создание бюджета, список, выбор активного; `IBudgetService.GetActiveBudget()`
- `BudgetRepository` — работа с таблицей `Budgets` в SQLite

**Тесты:** `BudgetApp.Tests` — `BudgetStoreTests`, `BudgetServiceTests`

**Ветка:** `dev_budget-creation`
