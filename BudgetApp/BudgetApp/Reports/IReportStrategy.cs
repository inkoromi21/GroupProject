namespace Budget_App.Reports {
  /// <summary>
  /// Strategy contract for building a budget report text.
  /// </summary>
  internal interface IReportStrategy {
    string Generate(ReportData data);
  }
}
