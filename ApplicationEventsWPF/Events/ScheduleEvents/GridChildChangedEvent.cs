using Prism.Events;

namespace ApplicationEventsWPF.Events.ScheduleEvents
{
    /// <summary>
    /// Ивент оповещает что коллекция занятий изменилась
    /// </summary>
    public class GridChildChangedEvent : PubSubEvent<object[]> { }
}
