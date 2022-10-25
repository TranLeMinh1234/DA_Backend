using BL.Interface;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Business
{
    public class BLTemplateGroupTask : BLBase, IBLTemplateGroupTask
    {
        private IDLTemplateGroupTask _iDLTemplateGroupTask;
        private ContextRequest _contextRequest;

        public BLTemplateGroupTask(IDLTemplateGroupTask iDLTemplateGroupTask, ContextRequest contextRequest) : base(iDLTemplateGroupTask, contextRequest)
        {
            _iDLTemplateGroupTask = iDLTemplateGroupTask;
            _contextRequest = contextRequest;
        }

        public TemplateGroupTask InsertCustom(TemplateGroupTask templateGroupTask)
        {
            templateGroupTask.CreatedByEmail = _contextRequest.GetEmailCurrentUser();
            templateGroupTask.CreatedTime = DateTime.Now;

            foreach (var process in templateGroupTask.ListProcess)
            {
                process.CreatedTime = DateTime.Now;
                process.CreatedByEmail = _contextRequest.GetEmailCurrentUser();

                _iDLTemplateGroupTask.Insert(process.ColumnSetting);
                _iDLTemplateGroupTask.Insert(process);
            }

            _iDLTemplateGroupTask.Insert(templateGroupTask);

            return templateGroupTask;
        }
    }
}
