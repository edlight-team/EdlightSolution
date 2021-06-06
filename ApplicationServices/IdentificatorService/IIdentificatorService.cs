namespace ApplicationServices.IdentificatorService
{
    public interface IIdentificatorService
    {
        /// <summary>
        /// Установить длину ИД
        /// </summary>
        /// <param name="length">Длина</param>
        void SetIDLength(int length);
        /// <summary>
        /// Создать ИД в виде строки A1B2C3D4...
        /// </summary>
        /// <returns></returns>
        string GetStringID();
        /// <summary>
        /// Создать ИД в виде числа 123456...
        /// </summary>
        /// <returns></returns>
        int GetIntID();
    }
}
