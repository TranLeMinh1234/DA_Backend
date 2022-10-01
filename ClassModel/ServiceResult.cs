using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel
{
    public class ServiceResult
    {
        public bool Success { get; set; } = true;
        public object Data { get; set; }
        public string[] ErrorCode { get; set; }
        public string[] ErrorCodeDev { get; set; }
    }
}
