using System.Windows;
using System.Windows.Controls;

namespace Styles.Extensions
{
    public static class UIElementExtension
    {
        /// <summary>
        /// Установить для элемента Row и Column
        /// </summary>
        /// <param name="element">Элемент</param>
        /// <param name="row">Строка</param>
        /// <param name="collumn">Столбец</param>
        /// <param name="rowSpan">Объединение строк</param>
        /// <param name="columnSpan">Объединение столбцов</param>
        public static void SetGridPosition(this UIElement element, int row, int collumn, int rowSpan = 0, int columnSpan = 0)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, collumn);
            if (rowSpan != 0) Grid.SetRowSpan(element, rowSpan);
            if (columnSpan != 0) Grid.SetColumnSpan(element, columnSpan);
        }
    }
}
