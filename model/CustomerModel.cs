using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.utilities;

namespace WpfApp2.model
{
    public class CustomerModel
    {
        public List<User> Users;

        public string[] sortTypes = new string[] { "Без Сортировки", "По убыванию", "По возрастанию" };


        public CustomerModel()
        {
            FillUsers();
        }

        public void FillUsers()
        {
            Users = new List<User>();
            foreach (var i in GetUsersFromDb())
            {
                Users.Add(i);
            }
        }

        public string clearFioFilter()
        {
            return "";
        }
        public string clearSortType()
        {
            return sortTypes[0];
        }

        public bool clearOnlyAdmins()
        {
            return false;
        }

        public List<User> GetUsers()
        {
            return Users;
        }

        public List<User> GetUsersFromDb()
        {
            return DbManager.getUsers();
        }

        public List<User> ChangeUsersByFilter(string FioFilter, string SortType, bool OnlyAdmins)
        {
            List<User> usersFilter = GetUsers().ToList();

            if (string.IsNullOrEmpty(FioFilter) == false)
            {
                usersFilter = usersFilter.Where(ch => ch.fio.ToLower().Contains(FioFilter.ToLower())).ToList();
            }
            if (string.IsNullOrEmpty(SortType) == false)
            {
                switch (SortType)
                {
                    case "Без Сортировки":
                        {
                            break;
                        }
                    case "По убыванию":
                        usersFilter = usersFilter.OrderByDescending(ch => ch.fio).ToList();
                        break;
                    case "По возрастанию":
                        usersFilter = usersFilter.OrderBy(ch => ch.fio).ToList();
                        break;
                }
            }

            usersFilter = OnlyAdmins == false ? usersFilter : usersFilter.Where(ch => ch.role == "Администратор").ToList();

            return usersFilter;
        }

    }
}
