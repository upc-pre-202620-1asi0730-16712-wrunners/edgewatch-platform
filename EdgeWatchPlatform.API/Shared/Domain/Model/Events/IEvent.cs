using Cortex.Mediator.Notifications;

namespace EdgeWatchPlatform.API.Shared.Domain.Model.Events;

/// <summary>
///     Marks a domain event. Events are collected by aggregate roots and dispatched through the mediator
///     when the unit of work is committed (see <c>DispatchDomainEventsInterceptor</c>).
/// </summary>
public interface IEvent : INotification;
