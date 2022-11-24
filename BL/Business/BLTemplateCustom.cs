using BL.Interface;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Business
{
    public class BLTemplateCustom : BLBase,IBLTemplateCustom
    {
        private IDLTemplateCustom _iDLTemplateCustom;
        private ContextRequest _contextRequest;
        private IBLTask _iBLTask;
        private WebsocketConnectionManager _websocketConnectionManager;

        public BLTemplateCustom(WebsocketConnectionManager websocketConnectionManager, IBLTask iBLTask, IDLTemplateCustom iDLTemplateCustom, ContextRequest contextRequest) : base(iDLTemplateCustom, contextRequest)
        {
            _iDLTemplateCustom = iDLTemplateCustom;
            _contextRequest = contextRequest;
            _iBLTask = iBLTask;
            _websocketConnectionManager = websocketConnectionManager;
        }

        public TemplateCustom InsertCustom(TemplateGroupTask templateGroupTask)
        {
            TemplateCustom templateCustom = new TemplateCustom() {
                TemplateCustomId = null,
                OriginTemplateId = templateGroupTask.TemplateGroupTaskId,
            };

            Guid? newTemplateCustomId = _iDLTemplateCustom.Insert(templateCustom);
            templateCustom.TemplateCustomId = newTemplateCustomId;
            if (newTemplateCustomId != null)
            {
                List<Process> ListProcess = templateGroupTask.ListProcess;
                foreach (var process in ListProcess)
                {
                    process.TemplateGroupTaskReferenceId = newTemplateCustomId;
                }

                InsertProcessBatch(ListProcess);
            }

            return templateCustom;
        }

        public int InsertProcessBatch(List<Process> listProcess)
        {
            var result = _iDLTemplateCustom.InsertProcessBatch(listProcess);
            return result;
        }

        public Process InsertProcess(Process process)
        {
            throw new NotImplementedException();
        }

        public int UpdateSortOrderProcesses(List<ParamUpdateSortOrderProcess> listParam)
        {
            throw new NotImplementedException();
        }
    }
}
