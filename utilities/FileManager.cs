using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.InteropServices;



namespace WpfApp2.utilities
{
    public static class FileManager
    {
        private static readonly string file_path = "json_data.json";
        private static readonly string image_folder_path = "images";
        private static readonly string standard_image = "standard_photo.jpg";
        public static void create_json()
        {
            // Проверяем существует ли файл
            if (!File.Exists(file_path))
            {
                // Создание основного json с данными аккаунта
                JObject json = new JObject
                    (
                    new JProperty("login", string.Empty),
                    new JProperty("password", string.Empty)
                    );
                // Записываем JSON в файл
                File.WriteAllText(file_path, json.ToString());
            }
        }

        public static void create_images_folder()
        {
            // Проверяем существует ли папка с изображениями
            if (!Directory.Exists(image_folder_path))
            {
                // Создаем папку
                Directory.CreateDirectory(image_folder_path);
            }
        }

        public static string get_standard_photo_url()
        {
            return standard_image;
        }

        public static void save_new_image(string path)
        {
            // путь к папке где хранятся картинки
            string imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "images");

            // путь к картинке которую чел выбрал
            string destinationFilePath = Path.Combine(imagesPath, Path.GetFileName(path));

            // копирование файла с перезаписью если нужно в папку
            File.Copy(path, destinationFilePath, overwrite: true);
        }

        public static bool checkJsonDataValid()
        {
            string login = FileManager.get_login();
            string password = FileManager.get_password();
            if (login == null || password == null) return false;

            using (var context = new MyDbContext())
            {
                // Проверяем, существует ли пользователь с заданным логином
                User user = context.Users.FirstOrDefault(u => u.login == login);
                if (user == null) return false;
                return user.password == Hash.GetHash(password);
            }
        }

        // метод для читки json файла
        private static JObject read_json()
        {
            if (!File.Exists(file_path)) return null;
            // Читаем файл
            string jsonContent = File.ReadAllText(file_path);
            // Парсим его
            JObject json = JObject.Parse(jsonContent);
            return json;
        }

        public static string get_login()
        {
            // Парсим JSON
            JObject json = read_json();
            if (json == null) return null;
            // Получаем значение ключа "name"
            string name = json["login"].ToString();
            return name;
        }

        // изменение имени
        public static void set_login(string login)
        {
            // Парсим JSON
            JObject json = read_json();
            // Изменяем значение ключа "name"
            json["login"] = login;
            File.WriteAllText(file_path, json.ToString());
        }

        // изменение имени
        public static void set_password(string password)
        {
            // Парсим JSON
            JObject json = read_json();
            // Изменяем значение ключа "name"
            json["password"] = password;
            File.WriteAllText(file_path, json.ToString());
        }
        public static string get_password()
        {
            // Парсим JSON
            JObject json = read_json();
            if (json == null) return null;
            // Получаем значение ключа "name"
            string name = json["password"].ToString();
            return name;
        }
    }
}
