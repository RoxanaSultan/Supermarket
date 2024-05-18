using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Models.Database;

namespace Supermarket.Models.BusinessLogicLayer
{
    public class UserBLL
    {
        private supermarketEntities context = new supermarketEntities();
        public string ErrorMessage { get; set; }
        public void AddUser(object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void UpdateUser(object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            User oldUser = context.Users.FirstOrDefault(u => u.user_id == user.user_id);
            if (oldUser == null)
            {
               ErrorMessage = "User not found!";
                return;
            }
            oldUser.name = user.name.Substring(0, 20);
            oldUser.password = user.password.Substring(0, 10);
            oldUser.type = user.type.Substring(0, 15);
            context.SaveChanges();
        }

        public void DeleteUser(object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            User oldUser = context.Users.FirstOrDefault(u => u.user_id == user.user_id);
            if (oldUser == null)
            {
                ErrorMessage = "User not found!";
                return;
            }
            if (oldUser.active == false)
            {
                ErrorMessage = "User is not active!";
                return;
            }
            context.Users.Remove(oldUser);
            context.SaveChanges();
        }

        public User GetUser(string username, string password)
        {
            return context.Users.FirstOrDefault(u => u.name == username && u.password == password);
        }

        public ObservableCollection<User> GetUsers()
        {
            return new ObservableCollection<User>(context.Users.Where(u => (bool)u.active).ToList());
        }

        public List<GetProfitPerUser_Result> GetProfitPerUser(int user_id, int month)
        {
            return context.GetProfitPerUser(user_id, month).ToList();
        }

    }
}
