using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp2.model;
using WpfApp2.utilities;

namespace WpfApp2.viewModel
{
    public class tableVM : utilities.MainModelView
    {
        private Navigation _navigation;
        private tableModel _tableModel;

        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        private ObservableCollection<User> users { get; set; }
        public ObservableCollection<User> Users 
        { 
            get { return users; }
            set { users = value; OnPropertyChanged(); }
        }

        public ICommand DeleteUserCommand { get; set; }
        public ICommand AddUserCommand { get; set; }

        public tableVM(Navigation navigation)
        {
            _tableModel = new tableModel();

            RefreshDataGridData();

            DeleteUserCommand = new RelayCommand(deleteUser);
            AddUserCommand = new RelayCommand(AddUser);


            _navigation = navigation;
        }

        private void RefreshDataGridData(bool refreshData = false)
        {
            if (Users != null)
            {
                foreach (var i in Users)
                {
                    i.PropertyChanged -= ChangeDbData;
                }
            }

            Users = new ObservableCollection<User>();
            foreach (var i in _tableModel.GetUsers(refreshData))
            {
                User user = new User
                {
                    id = i.id,
                    login = i.login,
                    password = i.password,
                    role = i.role,
                    fio = i.fio,
                    photo = i.photo
                };

                user.PropertyChanged += ChangeDbData;
                Users.Add(user);

            }
        }

        private void AddUser(object obj)
        {
            _navigation.CurrentView = new addMemberVM(_navigation);
        }

        private void deleteUser(object obj)
        {
            _tableModel.deleteUser(SelectedUser);

            Users.Remove(SelectedUser);
            SelectedUser = null;
        }

        private void ChangeDbData(object sender, PropertyChangedEventArgs e)
        {
            if (sender is User user)
            {
                switch (e.PropertyName)
                {
                    case "login":
                        {
                            if (_tableModel.checkUserByLogin(user.login) == false)
                            {
                                RefreshDataGridData();
                                MessageBox.Show("Неккоректный логин!");
                                return;
                            }
                            break;
                        }
                    case "password":
                        {
                            if (_tableModel.CheckPassword(user.password) == false)
                            {
                                RefreshDataGridData();
                                MessageBox.Show("Неккоректный пароль!");
                                return;
                            }
                            break;
                        }
                    case "role":
                        {
                            if (_tableModel.checkRole(user.role) == false)
                            {
                                RefreshDataGridData();
                                MessageBox.Show("Неккоректная роль!");
                                return;
                            }
                            break;
                        }
                    case "fio":
                        {
                            if (_tableModel.checkFio(user.fio) == false)
                            {
                                RefreshDataGridData();
                                MessageBox.Show("Неккоректное ФИО!");
                                return;
                            }
                            break;
                        }
                    case "photo":
                        {
                            if (_tableModel.checkPhoto(user.photo) == false)
                            {
                                RefreshDataGridData();
                                MessageBox.Show("Неккоректная фотография!");
                                return;
                            }
                            break;
                        }
                }

                _tableModel.UpdateDbData(users.ToList());

                RefreshDataGridData(true);

                // Здесь можно вызвать метод при изменении свойства
                MessageBox.Show($"Данные успешно обновлены!");
            }
        }
    }
}
