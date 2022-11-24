using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.TaskRelate
{
    public class TemplateCustom
    {
        [PrimaryKey]
        public Guid? TemplateCustomId { get; set; }
        [AddDatabase]
        public Guid? OriginTemplateId { get; set; }
        public List<Process> ListProcess { get; set; } = new List<Process>();
}
}
