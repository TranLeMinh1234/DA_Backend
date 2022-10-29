using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.ParamApi
{
    public class ParamInserGroupTask
    {
        public GroupTask GroupTask { get; set; }
        public List<ClassModel.User.User> ListUser { get; set; }
    }
}
