namespace Budget_App.Reports {
  internal interface IReportStrategy {
    string Generate(ReportData data);
  }
}
