using Prism.Events;

namespace ApplicationEventsWPF.Events.ScheduleEvents
{
    /// <summary>
    /// Ивент оповещает о том что изменились комментарии
    /// </summary>
    public class CommentsChangedEvent : PubSubEvent<string> { }
}
