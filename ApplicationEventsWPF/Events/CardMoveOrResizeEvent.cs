using Prism.Events;

namespace ApplicationEventsWPF.Events
{
    /// <summary>
    /// Ивент оповещает что размер или положение карточки изменилось
    /// </summary>
    public class CardMoveOrResizeEvent : PubSubEvent<HandyControl.Controls.Card> { }
}
