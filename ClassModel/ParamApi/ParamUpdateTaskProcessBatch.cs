using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.ParamApi
{
    public class ParamUpdateTaskProcessBatch
    {
        public Guid? TaskId { get; set; }
        public Guid? ProcessId { get; set; }
        public int? SortOrder { get; set; }
    }
}
