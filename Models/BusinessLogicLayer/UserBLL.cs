using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models.BusinessLogicLayer
{
    public class UserBLL
    {
        private supermarketEntities1 context = new supermarketEntities1();
        public string ErrorMessage { get; set; }
        public void AddUser(object obj)
        {
            User user = obj as User;
            if (user != null)
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
            oldUser.name = user.name;
            oldUser.password = user.password;
            oldUser.type = user.type;
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
            context.Users.Remove(oldUser);
            context.SaveChanges();
        }

        public User GetUser(string username, string password)
        {
            return context.Users.FirstOrDefault(u => u.name == username && u.password == password);
        }

        public ObservableCollection<User> GetUsers()
        {
            return new ObservableCollection<User>(context.Users.ToList());
        }

    }
}
