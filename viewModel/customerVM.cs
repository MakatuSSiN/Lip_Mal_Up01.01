using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.model;
using WpfApp2.Properties;
using WpfApp2.utilities;
using System.IO;
using System.Windows;

namespace WpfApp2.viewModel
{
    internal class customerVM : utilities.MainModelView
    {
        private Navigation _navigation;
        private CustomerModel _customerModel;

        private ObservableCollection<User> users { get; set; }
        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; OnPropertyChanged(); }
        }

        private string _fioFilter;

        public string FioFilter
        {
            get { return _fioFilter; }
            set 
            { 
                _fioFilter = value; 
                OnPropertyChanged();
                ChangeUsersByFilter(); 
            }
        }

        private string[] _sortTypes;
        
        public string[] SortTypes
        {
            get { return _sortTypes; }
            set {  _sortTypes = value; OnPropertyChanged(); }
        }

        private string _sorttype;

        public string SortType
        {
            get { return _sorttype; }
            set { _sorttype = value; OnPropertyChanged(); ChangeUsersByFilter(); }
        }

        private bool _onlyAdmins;

        public bool OnlyAdmins
        {
            get { return _onlyAdmins; }
            set { _onlyAdmins = value; OnPropertyChanged(); ChangeUsersByFilter(); }
        }

        public RelayCommand ClearFiltersCommand { get; set; }

        public customerVM(Navigation navigation)
        {
            _customerModel = new CustomerModel();
            _navigation = navigation;

            SortTypes = _customerModel.sortTypes;
            SortType = "Без Сортировки";
            OnlyAdmins = false;

            ClearFiltersCommand = new RelayCommand(clearFilters);

            FillUsers();
        }

        private void clearFilters(object obj)
        {
            FioFilter = _customerModel.clearFioFilter();
            SortType = _customerModel.clearSortType();
            OnlyAdmins = _customerModel.clearOnlyAdmins();
        }


        private void FillUsers()
        {
            Users = new ObservableCollection<User>();
            foreach (var i in _customerModel.GetUsers())
            {
                User user = new User
                {
                    id = i.id,
                    login = i.login,
                    password = i.password,
                    role = i.role,
                    fio = i.fio,
                    photo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", i.photo)
                };
                Users.Add(user);
            }
        }


        private void ChangeUsersByFilter()
        {
            List<User> usersFilter = _customerModel.ChangeUsersByFilter(FioFilter, SortType, OnlyAdmins);

            Users = new ObservableCollection<User>();
            foreach (var i in usersFilter)
            {
                User user = new User
                {
                    id = i.id,
                    login = i.login,
                    password = i.password,
                    role = i.role,
                    fio = i.fio,
                    photo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", i.photo)
                };
                Users.Add(user);
            }
        }
    }
}
