using System.Windows.Media;

namespace ApplicationModels.Config
{
    /// <summary>
    /// Json файл конфига цветов для типов занятий
    /// </summary>
    public class TypeClassColors
    {
        public Color LectionColor { get; set; }
        public Color LaboratoryColor { get; set; }
        public Color PracticeColor { get; set; }
        public Color ExaminationColor { get; set; }
        public Color DefaultColor { get; set; }
    }
}
