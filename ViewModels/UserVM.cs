using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Supermarket.Helpers;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using Supermarket.Views;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Supermarket.Models.Database;


namespace Supermarket.ViewModels
{
    class UserVM : BaseVM
    {
        private UserBLL userBLL;
        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            private set
            {
                users = value;
                NotifyPropertyChanged(nameof(Users));
            }
        }
        public UserVM()
        {
            userBLL = new UserBLL();
            users = new ObservableCollection<User>(userBLL.GetUsers());
        }

        public void AddUser(object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                userBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidUser(user))
            {
                userBLL.ErrorMessage = "Validation failed!";
                return;
            }
            userBLL.AddUser(user);
            if (string.IsNullOrEmpty(userBLL.ErrorMessage))
            {
                users.Add(user);
            }
        }

        private ICommand addUserCommand;
        public ICommand AddUserCommand
        {
            get
            {
                return addUserCommand ?? (addUserCommand = new RelayCommand(AddUser));
            }
        }

        public void UpdateUser(object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                userBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidUser(user))
            {
                userBLL.ErrorMessage = "Validation failed!";
                return;
            }
            userBLL.UpdateUser(user);
            if (string.IsNullOrEmpty(userBLL.ErrorMessage))
            {
                // Update the ObservableCollection
                var existingUser = Users.FirstOrDefault(p => p.user_id == user.user_id);
                if (existingUser != null)
                {
                    var index = Users.IndexOf(existingUser);
                    Users[index] = user; // This only works if the collection is bound to UI with bindings that detect changes.
                }
            }
        }

        private ICommand updateUserCommand;
        public ICommand UpdateUserCommand
        {
            get
            {
                return updateUserCommand ?? (updateUserCommand = new RelayCommand(UpdateUser));
            }
        }

        public void DeleteUser(object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                userBLL.ErrorMessage = "Invalid input!";
                return;
            }
            userBLL.DeleteUser(user);
            if (string.IsNullOrEmpty(userBLL.ErrorMessage))
            {
                users.Remove(user);
            }
        }

        private ICommand deleteUserCommand;
        public ICommand DeleteUserCommand
        {
            get
            {
                return deleteUserCommand ?? (deleteUserCommand = new RelayCommand(DeleteUser));
            }
        }

        private bool IsValidUser(User user)
        {
            if (string.IsNullOrEmpty(user.name) || string.IsNullOrEmpty(user.password) || string.IsNullOrEmpty(user.type))
            {
                return false;
            }
            return true;
        }

    }
}
