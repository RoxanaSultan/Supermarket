using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.ViewModels;
using Supermarket.Helpers;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models.Database;

namespace Supermarket.ViewModels
{
    class LoginVM: BaseVM
    {
        private UserBLL userBLL;
        public LoginVM()
        {
            userBLL = new UserBLL();
        }
        public bool ableToLogin(string username, string password, string type)
        {
            User user = userBLL.GetUser(username, password);
            return user != null && user.type.Trim() == type.Trim() && user.active;
        }
    }
}
