using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.model;
using WpfApp2.utilities;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace WpfApp2.viewModel
{
    public class addMemberVM : utilities.MainModelView
    {
        private Navigation _navigation;
        private AddUserModel _addUserModel;

        private string[] _roles;
        public string[] Roles
        {
            get { return _roles; }
            set { _roles = value; OnPropertyChanged(); }
        }
        public string DefaultRole { get; set; }

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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; OnPropertyChanged(); }
        }
        private string _fio;
        public string Fio
        {
            get { return _fio; }
            set { _fio = value; OnPropertyChanged(); }
        }

        private string _photo;

        public string Photo
        {
            get { return _photo; }
            set { _photo = value; OnPropertyChanged(); }
        }

        private string _photo_name;

        public string Photo_name
        {
            get { return _photo_name; }
            set { _photo_name = value; OnPropertyChanged(); }
        }


        public RelayCommand CancelCommand { get; set; }
        public RelayCommand AddUserCommand { get; set; }
        public RelayCommand AddPhotoCommand { get; set; }

        public addMemberVM(Navigation navigation) 
        { 
            _navigation = navigation;
            _addUserModel = new AddUserModel();
            Roles = _addUserModel.roles;
            if (Roles.Length > 1) DefaultRole = Roles[1];
            Photo_name = "Выберите фото";

            CancelCommand = new RelayCommand(CancelAddUser);
            AddUserCommand = new RelayCommand(AddUser);
            AddPhotoCommand = new RelayCommand(AddPhoto);

        }

        private void CancelAddUser(object obj)
        {
            if (FileManager.checkJsonDataValid()) _navigation.CurrentView = new tableVM(_navigation);
            else _navigation.CurrentView = new AuthorizationVM(_navigation);
        }

        private void AddPhoto(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "Изображения|*.jpg;*.jpeg;*.png;",
                Multiselect = false // Разрешить выбрать только один файл
            };

            // Открываем диалоговое окно
            bool? result = openFileDialog.ShowDialog();

            // Проверяем, был ли файл выбран
            if (result == true && !string.IsNullOrEmpty(openFileDialog.FileName))
            {
                Photo = openFileDialog.FileName;
                Photo_name = System.IO.Path.GetFileName(Photo);
            }
        }

        private void AddUser(object obj)
        {
            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (SearchUserByLogin())
            {
                MessageBox.Show("Такой логин уже есть!");
                return;
            }
            if (string.IsNullOrEmpty(Password) || CheckPassword() == false)
            {
                MessageBox.Show("Пароль должен:\nИметь длину минимум 6 символов.\nСодержать только английские буквы\nСодержать хотя бы одну цифру.");
                return;
            }
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            if (string.IsNullOrEmpty(Fio))
            {
                MessageBox.Show("Введите ФИО!");
                return;
            }

            if (CheckFio() == false)
            {
                MessageBox.Show("Неправильное Фио!\nПример заполнения(один из вариантов):\nИнициалы через точки (Л.)\nПолное слово через пробел");
                return;
            }

            if (string.IsNullOrEmpty(Photo) || Photo_name == "Выберите фото")
            {
                Photo_name = FileManager.get_standard_photo_url();
            }
            else
            {
                FileManager.save_new_image(Photo);
            }

            var newUser = new User
            {
                login = Login,
                password = Hash.GetHash(Password),
                role = DefaultRole,
                fio = Fio,
                photo = Photo_name
            };

            DbManager.addUser(newUser);

            if (string.IsNullOrEmpty(FileManager.get_login()))
            {
                FileManager.set_login(Login);
                FileManager.set_password(Password);

                if (newUser.role == "Администратор") _navigation.CurrentView = new tableVM(_navigation);
                else _navigation.CurrentView = new customerVM(_navigation);
            }
            _navigation.CurrentView = new tableVM(_navigation);
        }

        private bool SearchUserByLogin()
        {
            return DbManager.checkUserByLogin(Login);
        }

        private bool CheckPassword()
        {
            if (Password.Length < 6) return false;
            foreach(var i in Password)
            {
                if (char.IsLetter(i) && !((i >= 'a' && i <= 'z') || (i >= 'A' && i <= 'Z'))) return false;
            }
            if (Password.Where(ch => char.IsLetter(ch)).Count() == 0) return false;

            return true;
        }

        private bool CheckFio()
        {
            Fio = Fio.Trim();

            if (Fio.Count(ch => ch == ' ') == 2 && Fio.Count(ch => ch == '.') == 0)
            {
                var parts = Fio.Split(' ');
                return parts.Length == 3 && parts.All(part => !string.IsNullOrWhiteSpace(part));
            }

            if (Fio.Count(ch => ch == ' ') == 1 && Fio.Count(ch => ch == '.') == 2)
            {
                var parts = Fio.Split(' ');
                return parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0])
                       && parts[1].Count(ch => ch == '.') == 2;
            }
            return false;
        }
    }
}
