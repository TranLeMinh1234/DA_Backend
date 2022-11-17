using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.ParamApi
{
    public class ParamAddMember
    {
        public Guid? GroupTaskId { get; set; }
        public List<ClassModel.User.User> ListUser { get; set; }
    }
}
