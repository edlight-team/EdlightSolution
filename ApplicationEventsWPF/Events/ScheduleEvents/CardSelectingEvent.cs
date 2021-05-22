using Prism.Events;
using System.Collections.Generic;

namespace ApplicationEventsWPF.Events.ScheduleEvents
{
    /// <summary>
    /// Событие оповещает о том что карточка была выбрана/перестала быть выбранной
    /// </summary>
    public class CardSelectingEvent : PubSubEvent<KeyValuePair<string, bool>> { }
}
