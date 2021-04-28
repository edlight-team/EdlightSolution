using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class LessonsModel : BindableBase
    {
        #region fields

        private Guid id;
        private Guid idWeekDay;
        private Guid idTimeLessons;
        private Guid idTeacher;
        private Guid idAcademicDiscipline;
        private Guid idTypeClass;
        private Guid idAudience;
        private bool evenNumberedWeek;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(IdWeekDay))]
        public Guid IdWeekDay { get => idWeekDay; set => SetProperty(ref idWeekDay, value); }

        [JsonProperty(nameof(IdTimeLessons))]
        public Guid IdTimeLessons { get => idTimeLessons; set => SetProperty(ref idTimeLessons, value); }

        [JsonProperty(nameof(IdTeacher))]
        public Guid IdTeacher { get => idTeacher; set => SetProperty(ref idTeacher, value); }

        [JsonProperty(nameof(IdAcademicDiscipline))]
        public Guid IdAcademicDiscipline { get => idAcademicDiscipline; set => SetProperty(ref idAcademicDiscipline, value); }

        [JsonProperty(nameof(IdTypeClass))]
        public Guid IdTypeClass { get => idTypeClass; set => SetProperty(ref idTypeClass, value); }

        [JsonProperty(nameof(IdAudience))]
        public Guid IdAudience { get => idAudience; set => SetProperty(ref idAudience, value); }

        [JsonProperty(nameof(EvenNumberedWeek))]
        public bool EvenNumberedWeek { get => evenNumberedWeek; set => SetProperty(ref evenNumberedWeek, value); }

        #endregion
    }
}
