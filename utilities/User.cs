using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.utilities
{
    public class User : MainModelView
    {
        private int _id;
        [Key] public int id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
                OnPropertyChanged(); 
            }
        }

        private string _login;
        public string login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged();
                }
                OnPropertyChanged();
            }
        }

        private string _password;
        public string password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }

            }
        }

        private string _role;
        public string role
        {
            get { return _role; }
            set
            {
                if (_role != value)
                {
                    _role = value;
                    OnPropertyChanged();
                }

            }
        }

        private string _fio;
        public string fio
        {
            get { return _fio; }
            set
            {
                if (_fio != value)
                {
                    _fio = value;
                    OnPropertyChanged();
                }

            }
        }

        private string _photo;
        public string photo
        {
            get { return _photo; }
            set
            {
                if (_photo != value)
                {
                    _photo = value;
                    OnPropertyChanged();
                }

            }
        }
    }
}
