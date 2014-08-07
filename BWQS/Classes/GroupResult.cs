using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace System.Linq.Dynamic.BitWise.Service
{
    [Serializable]
    [DataContract]
    public class GroupResult
    {
        [DataMember]
        public object Key { get; set; }

        [DataMember]
        public object Values { get; set; }
    }
}
