using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp2.utilities;

namespace WpfApp2.viewModel
{
    public class AuthorizationVM : utilities.MainModelView
    {
        private Navigation _navigation;

        private string _login;

        public string Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged(); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
         
        public ICommand goToRegistrationCommand { get; set; }
        public ICommand authorizationCommand { get; set; }

        public AuthorizationVM(Navigation navigation)
        {
            _navigation = navigation;

            goToRegistrationCommand = new RelayCommand(goToRegistration);
            authorizationCommand = new RelayCommand(Authorization);
        }


        private void goToRegistration(object obj)
        {
            _navigation.CurrentView = new addMemberVM(_navigation);
        }
        private void Authorization(object obj)
        {
            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Введите логин!");
                return;
            }
            if (DbManager.checkUserByLogin(Login) == false)
            {
                MessageBox.Show("Пользователя с таким логином нет!");
                return;
            }
            
            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Введите пароль!");
                return;
            }
            
            User user = DbManager.GetUserByLogin(Login);

            if (user.password != Hash.GetHash(Password))
            {
                MessageBox.Show("Неверный пароль!");
                return;
            }

            FileManager.set_login(Login);
            FileManager.set_password(Password);
            if (user.role == "Администратор") _navigation.CurrentView = new tableVM(_navigation);
            else _navigation.CurrentView = new customerVM(_navigation);
        }
    }
}
