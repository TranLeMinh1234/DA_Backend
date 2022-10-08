using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel
{
    public class ServiceResult
    {
        public bool Success { get; set; } = true;
        public object Data { get; set; }
        public List<string> ErrorCode { get; set; } = new List<string>();
        public List<string> ErrorCodeDev { get; set; } = new List<string>();
    }
}
