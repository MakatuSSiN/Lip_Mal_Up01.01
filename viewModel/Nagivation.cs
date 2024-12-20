using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApp2.utilities;
using System.Globalization;
using System.Windows.Input;

namespace WpfApp2.viewModel
{
    public class Navigation : utilities.MainModelView
    {
        // текущий вид окна
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set 
            {
                if (_currentView != value) 
                {
                    UpdatePreviousPage();

                    _currentView = value;

                    OnPropertyChanged();
                    UpdateCurrentPage();
                }
            }
        }

        private string _currentDataTime;

        public string CurrentDataTime
        {
            get { return _currentDataTime; }
            set { _currentDataTime = value; OnPropertyChanged(); }
        }

        private string _currentPage;

        public string CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged(); }
        }

        private Stack<object> _previousPages = new Stack<object>();

        public Stack<object> PreviousPages
        {
            get { return _previousPages; }
            set { _previousPages = value; OnPropertyChanged(); }
        }


        public RelayCommand goToPreviousPageCommand { get; set; }

        public Navigation()
        {
            if (FileManager.checkJsonDataValid())
            {
                User user = DbManager.GetUserByLogin(FileManager.get_login());

                if (user.role == "Администратор") _currentView = new tableVM(this);
                else _currentView = new customerVM(this);
            }
            else _currentView = new AuthorizationVM(this);

            goToPreviousPageCommand = new RelayCommand(goToPreviousPage);

            UpdateCurrentDateTime();
            UpdateCurrentPage();
        }

        private async Task UpdateCurrentDateTime()
        {
            while (true)
            {
                CurrentDataTime = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss", new CultureInfo("ru-RU"));
                await Task.Delay(1000);
            }
        }

        private void UpdateCurrentPage()
        {
            if (_currentView is tableVM) CurrentPage = "Липовецкий_Мальков_ИСИП-321_админ";
            else if (_currentView is AuthorizationVM) CurrentPage = "Липовецкий_Мальков_ИСИП-321_Авторизация";
            else if (_currentView is addMemberVM) CurrentPage = "Липовецкий_Мальков_ИСИП-321_Регистрация";
            else CurrentPage = "Липовецкий_Мальков_ИСИП-321_Не_админ";
        }

        private void UpdatePreviousPage()
        {
            if (CurrentView is addMemberVM)
            {
                PreviousPages.Push(new addMemberVM(this));
            }
            else if (CurrentView is tableVM)
            {
                PreviousPages.Push(new tableVM(this));
            }
            else if (CurrentView is customerVM)
            {
                PreviousPages.Push(new customerVM(this));
            }
        }

        private void goToPreviousPage(object obj)
        {
            if (PreviousPages.Count > 0 && !PreviousPages.Contains(CurrentView))
            {
                // Получаем последнюю страницу
                object lastPage = PreviousPages.Pop();
                CurrentView = lastPage;
                PreviousPages.Pop();
            }
        }
    }
}
