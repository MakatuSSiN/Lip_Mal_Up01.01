using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.utilities;

namespace WpfApp2.model
{
    public class tableModel
    {

        public List<User> Users;

        public tableModel()
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

        public List<User> GetUsers(bool UpdateData = false)
        {
            if (UpdateData == false) return Users;
            else
            {
                FillUsers();
                return Users;
            }
        }
        public List<User> GetUsersFromDb()
        {
            return DbManager.getUsers();
        }

        public void deleteUser(User SelectedUser)
        {
            if (SelectedUser != null)
            {
                DbManager.deleteUserById(SelectedUser.id);

                Users.Remove(SelectedUser);
            }
        }

        public bool checkUserByLogin(string login)
        {
            return DbManager.checkUserByLogin(login);
        }

        public bool CheckPassword(string password)
        {
            if (password.Length < 6) return false;
            foreach (var i in password)
            {
                if (char.IsLetter(i) && !((i >= 'a' && i <= 'z') || (i >= 'A' && i <= 'Z'))) return false;
            }
            if (password.Where(ch => char.IsLetter(ch)).Count() == 0) return false;

            return true;
        }

        // не знаю какую проверку сделать, просто затычка
        public bool checkRole(string role)
        {
            return role.Length < 150;
        }

        public bool checkFio(string fio)
        {
            fio = fio.Trim();

            if (fio.Count(ch => ch == ' ') == 2 && fio.Count(ch => ch == '.') == 0)
            {
                var parts = fio.Split(' ');
                return parts.Length == 3 && parts.All(part => !string.IsNullOrWhiteSpace(part));
            }

            if (fio.Count(ch => ch == ' ') == 1 && fio.Count(ch => ch == '.') == 2)
            {
                var parts = fio.Split(' ');
                return parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0])
                       && parts[1].Count(ch => ch == '.') == 2;
            }
            return false;
        }

        // не знаю какую проверку сделать, просто затычка
        public bool checkPhoto(string photo)
        {
            return photo.Length < 150;

        }

        public void UpdateDbData(List<User> users)
        {
            DbManager.UpdateDbData(users);
            GetUsersFromDb();
        }
    }
}
