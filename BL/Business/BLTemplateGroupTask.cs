﻿using BL.Interface;
using ClassModel;
using ClassModel.ParamApi;
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
                process.TemplateGroupTaskReferenceId = templateGroupTask.TemplateGroupTaskId;
                process.ColumnSettingReferenceId = process.ColumnSetting.ColumnSettingId;

                _iDLTemplateGroupTask.Insert(process.ColumnSetting);
                _iDLTemplateGroupTask.Insert(process);
            }

            _iDLTemplateGroupTask.Insert(templateGroupTask);

            ClassModel.User.User user = _iDLTemplateGroupTask.GetUserInfo(_contextRequest.GetEmailCurrentUser());
            templateGroupTask.CreatedBy = user;
            

            return templateGroupTask;
        }
        public List<TemplateGroupTask> GetAllTemplate() {
            var result = _iDLTemplateGroupTask.GetAllTemplate(_contextRequest.GetEmailCurrentUser());
            return result;
        }

        public int DeleteCustom(Guid templateId) {
            var result = _iDLTemplateGroupTask.DeleteCustom(templateId);

            return result;
        }

        public Guid? UpdateProcess(Process process) {
            var resultUpdateProcess = _iDLTemplateGroupTask.Update(process);
            var resultUpdateColumnSetting = _iDLTemplateGroupTask.Update(process.ColumnSetting);
            return resultUpdateProcess;
        }

        public ClassModel.TaskRelate.Process InsertProcess(ClassModel.TaskRelate.Process process) {
            process.ColumnSettingReferenceId = process.ColumnSetting.ColumnSettingId;
            process.CreatedByEmail = _contextRequest.GetEmailCurrentUser();
            process.CreatedTime = DateTime.Now;

            Process lastestProcess = _iDLTemplateGroupTask.GetLastestProcess((Guid)process.TemplateGroupTaskReferenceId);

            if (lastestProcess != null)
            {
                process.SortOrder = lastestProcess.SortOrder + 1;
            }

            _iDLTemplateGroupTask.Insert(process);
            _iDLTemplateGroupTask.Insert(process.ColumnSetting);
            return process;
        }

        public ServiceResult DeleteProcess(Guid processId, Guid columnSettingId) {
            ServiceResult serviceResult = new ServiceResult();
            bool isExistTaskInProcess = _iDLTemplateGroupTask.CheckExistsTaskInProcess(processId);

            if (isExistTaskInProcess)
            {
                serviceResult.Success = false;
                serviceResult.ErrorCode.Add("ExistsTaskInProcess");
            }
            else
            {
                _iDLTemplateGroupTask.Delete<Process>(processId);
                _iDLTemplateGroupTask.Delete<Process>(columnSettingId);
            }
            return serviceResult;
        }

        public int UpdateSortOrderProcesses(List<ParamUpdateSortOrderProcess> listParam) {
            var result = _iDLTemplateGroupTask.UpdateSortOrderProcesses(listParam);
            return result;
        }
    }
}
