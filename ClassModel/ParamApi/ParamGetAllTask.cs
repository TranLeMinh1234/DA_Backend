using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.ParamApi
{
    public class ParamGetAllTask
    {
        public Guid? GroupTaskId { get; set; }
        public DateTime? FromStartTime { get; set; }
        public DateTime? ToStartTime { get; set; }
        public DateTime? FromEndTime { get; set; }
        public DateTime? ToEndTime { get; set; }
        public string NameTask { get; set; }
        public int? StatusComplete { get; set; }
        public string ExecutingUserEmail { get; set; }
    }
}
