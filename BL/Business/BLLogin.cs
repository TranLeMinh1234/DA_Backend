using BL.Interface;
using ClassModel;
using ClassModel.User;
using DL;
using DL.Interface;
using Microsoft.IdentityModel.Tokens;
using Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utilities;

namespace BL.Business
{
    public class BLLogin : BLBase, IBLLogin
    {
        protected IDLLogin _dlLogin;
        private ContextRequest _contextRequest;
        public BLLogin(IDLLogin dLLogin, ContextRequest contextRequest) : base(dLLogin, contextRequest) {
            _dlLogin = dLLogin;
            _contextRequest = contextRequest;
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


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public ServiceResult Login(Account account, string keyJwt)
        {
            ServiceResult result = new ServiceResult();
            if (string.IsNullOrEmpty(account.Email))
            {
                result.Success = false;
                result.ErrorCode.Add("EmptyEmail");
            }
            else if (string.IsNullOrEmpty(account.PassWord))
            {
                result.Success = false;
                result.ErrorCode.Add("EmptyPassWord");
            }

            User user = _dlLogin.GetUserByEmail(account.Email);
            if (user != null)
            {
                string hashPassWord = HashUtility.GetHashMd5(account.PassWord);
                if (hashPassWord == user.PassWord)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyJwt));
                    var creadentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    List<Claim> listClaim = new List<Claim>();
                    listClaim.Add(new Claim("userEmail", user.Email));
                    var token = new JwtSecurityToken(null,
                        expires: DateTime.Now.AddMinutes(120),
                        claims: listClaim,
                        signingCredentials: creadentials);

                    result.Data = new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    result.Success = false;
                    result.ErrorCode.Add("WrongPassWord");
                }
            }
            else
            {
                result.Success = false;
                result.ErrorCode.Add("WrongEmail");
            }

            return result;
        }
    }
}
