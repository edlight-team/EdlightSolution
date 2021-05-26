using System;

namespace ApplicationEventsWPF.Events.Signal
{
    public class EntitySignalModel
    {
        public string SendType { get; set; }
        public Type ModelType { get; set; }
        public string SerializedModel { get; set; }
    }
}
