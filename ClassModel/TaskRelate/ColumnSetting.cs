using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.TaskRelate
{
    public class ColumnSetting
    {
        [PrimaryKey]
        public Guid? ColumnSettingId { get; set; }
        [AddDatabase]
        public string Color { get; set; }
        [AddDatabase]
        public string ColorText { get; set; }
        [AddDatabase]
        public string ColorHeader { get; set; }
    }
}
