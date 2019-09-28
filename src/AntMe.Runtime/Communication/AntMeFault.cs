using System.Runtime.Serialization;

namespace AntMe.Runtime.Communication
{
    [DataContract]
    public class AntMeFault
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
