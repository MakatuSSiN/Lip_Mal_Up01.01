using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.utilities
{
    public static class DbManager
    {
        public static List<User> getUsers()
        {
            using (var context = new MyDbContext())
            {
                return context.Users.ToList();
            }
        }

        public static void addUser(User user)
        {
            using (var context = new MyDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static bool checkUserByLogin(string Login)
        {
            using (var context = new MyDbContext())
            {
                // Проверяем, существует ли пользователь с заданным логином
                return context.Users.Any(u => u.login == Login);
            }
        }
        public static User GetUserByLogin(string Login)
        {
            using (var context = new MyDbContext())
            {
                // Проверяем, существует ли пользователь с заданным логином
                return context.Users.First(u => u.login == Login);
            }
        }

        public static void deleteUserById(int id)
        {
            using (var context = new MyDbContext())
            {
                // Удаляем пользователя из базы данных
                var userToRemove = context.Users.Find(id);
                if (userToRemove != null)
                {
                    context.Users.Remove(userToRemove);
                    context.SaveChanges();
                }
            }
        }

        public static void UpdateDbData(List<User> users)
        {
            using (var context = new MyDbContext())
            {
                foreach (var user in users)
                {
                    var existingUser = context.Users.SingleOrDefault(u => u.id == user.id);

                    existingUser.login = user.login;
                    existingUser.password = user.password;
                    existingUser.role = user.role;
                    existingUser.fio = user.fio;
                    existingUser.photo = user.photo;
                }
                context.SaveChanges();
            }
        }
    }
}
