using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.ParamApi
{
    public class ParamUpdateDeadline
    {
        public DateTime? newDeadline { get; set; }

        public int typeDeadline { get; set; }

        public Guid taskId { get; set; }
    }
}
