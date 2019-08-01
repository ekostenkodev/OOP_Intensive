using System;
using System.Collections.Generic;
using System.Linq;

namespace Naming
{
    /*
        Именование
        Опишите класс который хранит пользователей системы. Через объект этого класса мы можем:
        1)Получить пользователя по имени
        2)Получить пользователя по Id
        3)Получить всех пользователей
        4)Получить пользователей у которых зарплата больше N
        5)Получить пользователей у которых зарплата меньше N
        6)Получить пользователей у которых запралта от N1 до N2
    */
    class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Salary { get; private set; }

        public User(int id, string name, decimal salary)
        {
            Id = id;
            Name = name;
            Salary = salary;
        }


    }
    class Users
    {
        private List<User> _users = new List<User>();

        public Users(List<User> users)
        {
            _users = users;
        }

        public User GetUser(string name) => _users.FirstOrDefault(e => e.Name.Equals(name));
        public User GetUser(int id) => _users.FirstOrDefault(e => e.Id == id);
        public IEnumerable<User> GetAllUsers() => _users;
        public IEnumerable<User> GetUserBySalaryGreaterThen(decimal salary) => _users.Where(item => item.Salary > salary).ToList();
        public IEnumerable<User> GetUserBySalaryLowerThen(decimal salary) => _users.Where(item => item.Salary < salary).ToList();
        public IEnumerable<User> GetUsersBySalaryInterval(decimal minSalary, decimal maxSalary) => _users.Where(item => item.Salary > minSalary && item.Salary < maxSalary).ToList();
    }
    class Naming
    {
        static void Main(string[] args)
        {
            List<User> userList = new List<User>()
            {   new User(1, "Egor", 10),
                new User(2, "Roma", 20),
                new User(3, "Boris", 5),
            };
            Users users = new Users(userList);

            Console.WriteLine($"1) Id Егора : {users.GetUser("Egor").Id}");

            Console.WriteLine($"2) Зарплата Ромы : {users.GetUser(2).Salary}");

            var selectedUsers = users.GetAllUsers();
            var usersString = String.Join(", ", selectedUsers.Select(item => item.Name).ToArray());
            Console.WriteLine($"3) Все пользователи : {usersString}");

            int N = 10;
            selectedUsers = users.GetUserBySalaryGreaterThen(N);
            usersString = String.Join(", ", selectedUsers.Select(item => item.Name).ToArray());
            Console.WriteLine($"4) Все пользователи, у кого зарплата больше {N} : {usersString}");

            selectedUsers = users.GetUserBySalaryLowerThen(N);
            usersString = String.Join(", ", selectedUsers.Select(item => item.Name).ToArray());
            Console.WriteLine($"5) Все пользователи, у кого зарплата меньше {N} : {usersString}");

            int N1 = 5, N2 = 25;
            selectedUsers = users.GetUsersBySalaryInterval(N1, N2);
            usersString = String.Join(", ", selectedUsers.Select(item => item.Name).ToArray());
            Console.WriteLine($"6) Все пользователи, у кого зарплата от {N1} до {N2}: {usersString}");

            Console.ReadKey();
        }
    }
}
