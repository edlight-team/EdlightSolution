using Prism.Events;

namespace ApplicationEventsWPF.Events
{
    /// <summary>
    /// Ивент оповещает о том что запись в справочниках была изменена
    /// </summary>
    public class DictionaryModelChangedEvent : PubSubEvent<object> { }
}
