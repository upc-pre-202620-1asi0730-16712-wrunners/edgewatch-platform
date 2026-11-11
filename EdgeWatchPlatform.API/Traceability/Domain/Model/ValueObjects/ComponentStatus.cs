namespace EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

/// <summary>Lifecycle status of a recovered component (RECEIVED, IN_RECUPERATION, ...).</summary>
public enum ComponentStatus { Received, InRecuperation, InService, Returned, Scrapped }
