using BL.Interface;
using ClassModel;
using ClassModel.User;
using DL;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace BL.Business
{
    public class BLLogin : BLBase, IBLLogin
    {
        protected IDLLogin _dlLogin;
        public BLLogin(IDLLogin dLLogin) : base(dLLogin) {
            _dlLogin = dLLogin;
        }

        /// <summary>
        /// Đăng ký user mới
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public ServiceResult Register(User newUser)
        {
            ServiceResult result = new ServiceResult();

            if (string.IsNullOrEmpty(newUser.Email))
            {
                result.Success = false;
                result.ErrorCode.Add("EmailEmpty");
            }
            else if (string.IsNullOrEmpty(newUser.PassWord))
            {
                result.Success = false;
                result.ErrorCode.Add("PasswordEmpty");
            }
            else if (newUser.PassWordRepeated != newUser.PassWord)
            {
                result.Success = false;
                result.ErrorCode.Add("PassWordRepeatedNotMatch");
            }

            if (_dlLogin.checkEmailRegisterExists(newUser.Email))
            {
                result.Success = false;
                result.ErrorCode.Add("EmailExists");
            }

            if (result.Success)
            {
                newUser.PassWord = HashUtility.GetHashMd5(newUser.PassWord);
                result.Data = _dlLogin.Insert(newUser);
            }

            return result;
        }
    }
}
