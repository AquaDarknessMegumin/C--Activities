using System.Collections.Generic;
using System.Linq;
using ToDo.Models;

namespace ToDo.Services
{
    public static class UserService
    {
        private static List<UserModel> _users = new List<UserModel>();
        private static UserModel _currentUser;
        private static int _nextId = 1;

        public static bool Register(string name, string email, string password)
        {
            if (_users.Any(u => u.Email == email))
                return false;

            _users.Add(new UserModel
            {
                UserId = _nextId++,
                Name = name,
                Email = email,
                Password = password
            });
            return true;
        }

        public static bool Login(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
                return false;

            _currentUser = user;
            return true;
        }

        public static void Logout()
        {
            _currentUser = null;
        }

        public static UserModel GetCurrentUser()
        {
            return _currentUser;
        }
    }
}
