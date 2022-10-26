using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLTemplateGroupTask : IBLBase
    {
        public TemplateGroupTask InsertCustom(TemplateGroupTask templateGroupTask);

        public List<TemplateGroupTask> GetAllTemplate();

        public int DeleteCustom(Guid templateId);
    }
}
