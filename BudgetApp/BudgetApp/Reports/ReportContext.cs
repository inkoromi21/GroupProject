namespace Budget_App.Reports {
  internal class ReportContext {
    private IReportStrategy strategy;
    private readonly ReportData reportData;

    public ReportContext(ReportData reportData) {
      this.reportData = reportData;
    }

    public void SetStrategy(IReportStrategy newStrategy) {
      strategy = newStrategy;
    }

    public string GenerateReport() {
      if (strategy == null) {
        return "Стратегия отчёта не выбрана.";
      }
      string reportText = strategy.Generate(reportData);
      return reportText;
    }
  }
}
