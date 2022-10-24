using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class ContextRequest
    {
        private string Email;

        public ContextRequest()
        {
            Console.WriteLine("asdasd");
        }

        public void SetEmailCurrentUser(string email)
        {
            Email = email;
        }

        public string GetEmailCurrentUser()
        {
            return Email;
        }
    }
}
