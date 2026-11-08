using System.ComponentModel.DataAnnotations;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Validation;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Request body to change the status of a recuperation (other properties, such as linkedSessions, are ignored).</summary>
public record ChangeRecuperationStatusResource(
    [Required(ErrorMessage = "RequiredStatus")] [EnumValue<RecuperationStatus>(ErrorMessage = "InvalidRecuperationStatus")] string Status);
