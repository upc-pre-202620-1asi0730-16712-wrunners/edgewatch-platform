using Cortex.Mediator.Notifications;
using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Shared.Application.Internal.EventHandlers;

/// <summary>Handler of a domain event, discovered and invoked by the mediator.</summary>
public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent;
