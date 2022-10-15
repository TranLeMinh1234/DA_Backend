using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLLabel : IBLBase
    {
        public Label InsertLabelCustom(Label newLabel);
        public Label UpdateLabelCustom(Label newLabel);
    }
}
