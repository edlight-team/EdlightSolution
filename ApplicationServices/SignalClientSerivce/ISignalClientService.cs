using System;
using System.Threading.Tasks;

namespace ApplicationServices.SignalClientSerivce
{
    public interface ISignalClientService
    {
        #region Subscribes

        /// <summary>
        /// Подписка на события изменений сущностей
        /// </summary>
        /// <param name="handler">Обработчик сообщений</param>
        /// <returns></returns>
        Task SubscribeEntityChanged(Action<string> handler);

        #endregion
        #region Unsubscribes

        /// <summary>
        /// Отписка от событий изменений сущностей
        /// </summary>
        /// <returns></returns>
        Task UnsubscribeEntityChanged();

        #endregion
        #region Sending

        /// <summary>
        /// Отправка события cущность изменена
        /// </summary>
        /// <param name="serializedEntity">сериализованная сущность</param>
        /// <returns></returns>
        Task SendEntityModel(string serializedEntity);

        #endregion
    }
}
