using ApplicationModels.Models;
using Prism.Events;
using System.Collections.Generic;

namespace ApplicationEventsWPF.Events.ScheduleEvents
{
    /// <summary>
    /// Ивент оповещает о том что изменились комментарии
    /// </summary>
    public class CommentsChangedEvent : PubSubEvent<KeyValuePair<string, IEnumerable<CommentModel>>> { }
}
