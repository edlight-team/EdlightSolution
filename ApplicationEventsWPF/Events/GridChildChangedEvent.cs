using Prism.Events;

namespace ApplicationEventsWPF.Events
{
    /// <summary>
    /// Ивент оповещает что коллекция занятий изменилась
    /// </summary>
    public class GridChildChangedEvent : PubSubEvent<object[]> { }
}
