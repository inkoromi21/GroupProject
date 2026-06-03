namespace Budget_App.Reports {
  /// <summary>
  /// Context that delegates report generation to a selected strategy.
  /// </summary>
  internal class ReportContext {
    private IReportStrategy strategy;
    private ReportData reportData;

    public ReportContext(ReportData reportData) {
      this.reportData = reportData;
    }

    /// <summary>
    /// Sets the report generation strategy.
    /// </summary>
    public void SetStrategy(IReportStrategy newStrategy) {
      strategy = newStrategy;
    }

    /// <summary>
    /// Builds a report using the current strategy.
    /// </summary>
    public string GenerateReport() {
      if (strategy == null) {
        return "Report strategy is not selected.";
      }
      string reportText = strategy.Generate(reportData);
      return reportText;
    }
  }
}
