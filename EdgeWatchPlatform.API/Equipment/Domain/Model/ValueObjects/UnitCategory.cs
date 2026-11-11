namespace EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

/// <summary>Physical category of a parameter unit, exposed in lower snake case (flow, mass_flow, ...).</summary>
public enum UnitCategory { Flow, MassFlow, Pressure, Temperature, Speed, Dimensionless }
