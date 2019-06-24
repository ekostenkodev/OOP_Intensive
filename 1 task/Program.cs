using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_task
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

        public User GetUser(string name) => _users.Find(e => e.Name.Equals(name));
        public User GetUser(int id) => _users.Find(e => e.Id == id);
        public List<User> GetUsers()
        {
            return _users;
            // так нарушается принцип инкапсуляции же, нет? Будет доступ ко всем пользователям, делай что хочешь.
            // поэтому надо возвращать копию списка? как показано ниже
            // List<User> userListCopy = new List<User>();
            // _users.ForEach(item => userListCopy.Add(item)); // нужно еще делать клон объекта ( например, за счет нитерфейса ICloneable для класса юзера)
            // return userListCopy;
        }
        public List<User> GetUsers(decimal N, bool above)
        {
            // удобный, но излишний функционал
            //public List<User> GetUsers(decimal N, Func<decimal, decimal, bool> compare)
            //{
            //    return _users.Where(item => compare(item.Salary,N)).ToList();
            //}

            if (above)
                return _users.Where(item => item.Salary > N).ToList();
            else
                return _users.Where(item => item.Salary < N).ToList();
        }
        public List<User> GetUsers(decimal N1, decimal N2) => _users.Where(item => item.Salary > N1 && item.Salary < N2).ToList();
    }
    class Naming
    {
        static void Main(string[] args)
        {
            List<User> userList = new List<User>()
            {   new User(1, "Lepeha", 10),
                new User(2, "Roma", 20),
                new User(3, "Boris", 5),
            };
            Users users = new Users(userList);

            Console.WriteLine($"1) Id Лехи : {users.GetUser("Lepeha").Id}");

            Console.WriteLine($"2) Зарплата Ромы : {users.GetUser(2).Salary}");

            var selectedUsers = users.GetUsers();
            var usersString = String.Join(", ", selectedUsers.Select(item => item.Name).ToArray());
            Console.WriteLine($"3) Все пользователи : {usersString}");

            int N = 10;
            selectedUsers = users.GetUsers(N, true);
            usersString = String.Join(", ", selectedUsers.Select(item => item.Name).ToArray());
            Console.WriteLine($"4) Все пользователи, у кого зарплата больше {N} : {usersString}");

            selectedUsers = users.GetUsers(N, false);
            usersString = String.Join(", ", selectedUsers.Select(item => item.Name).ToArray());
            Console.WriteLine($"5) Все пользователи, у кого зарплата меньше {N} : {usersString}");

            int N1 = 5, N2 = 25;
            selectedUsers = users.GetUsers(N1,N2);
            usersString = String.Join(", ", selectedUsers.Select(item => item.Name).ToArray());
            Console.WriteLine($"6) Все пользователи, у кого зарплата от {N1} до {N2}: {usersString}");

            Console.ReadKey();
        }
    }
}
