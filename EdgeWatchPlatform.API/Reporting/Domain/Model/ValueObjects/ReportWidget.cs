namespace EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

/// <summary>One block of a section: a variable, how it is aggregated and how it is drawn.</summary>
public record ReportWidget(int Position, VariableSource VariableSource, string VariableCode, string VariableLabel, WidgetViewType ViewType, Aggregation Aggregation, WidgetWidth Width)
{
    public ReportWidget Validated()
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(Position, 1);
        ArgumentException.ThrowIfNullOrWhiteSpace(VariableCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(VariableLabel);
        return this with { VariableCode = VariableCode.Trim(), VariableLabel = VariableLabel.Trim() };
    }
}
