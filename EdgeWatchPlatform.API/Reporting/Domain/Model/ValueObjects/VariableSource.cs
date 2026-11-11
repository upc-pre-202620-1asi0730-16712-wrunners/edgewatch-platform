namespace EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

/// <summary>Where a widget takes its data from: process readings, the session, the recuperation order or the fault cases.</summary>
public enum VariableSource
{
    Process,
    Session,
    Recuperation,
    FaultCases
}
