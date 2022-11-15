using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.ParamApi
{
    public class ParamDeletGroupTask
    {
        public List<Guid> ListTaskId { get; set; }
        public Guid? GroupTaskId { get; set; }
    }
}
