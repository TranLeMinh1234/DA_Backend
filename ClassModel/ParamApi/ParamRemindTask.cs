using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.ParamApi
{
    public class ParamRemindTask
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double TimeBeforeEndTime { get; set; }
        public double TimeBeforeStartTime { get; set; }
        public Guid TaskId { get; set; }
        public List<string> EmailWillSend { get; set; }
        public Boolean IsRemindEndTime { get; set; }
        public Boolean IsRemindStartTime { get; set; }
    }
}
