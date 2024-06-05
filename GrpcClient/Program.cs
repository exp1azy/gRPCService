using Grpc.Net.Client;

namespace GrpcClient
{
    internal class Program
    {
        static Greeter.GreeterClient _client;
        static List<User> _users;

        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7168");
            _client = new Greeter.GreeterClient(channel);

            while (true)
            {
                Console.WriteLine("Выберите операцию:\n" +
                "1) Получить список пользователей\n" +
                "2) Создать пользователя\n" +
                "3) Редактировать пользователя\n" +
                "4) Удалить пользователя");

                Console.WriteLine("\n------------------------------------------\n");

                var request = Console.ReadLine();

                Console.Clear();

                if (request == "1")
                {
                    var result = await GetUsersAsync();

                    foreach (var user in result)
                        Console.WriteLine($"{user.Id}: {user.Username} | {user.Password}"); 
                    
                    _users = result;
                }
                if (request == "2")
                {
                    string? username = null;
                    string? password = null;

                    while (username == null)
                    {
                        Console.WriteLine("Введите имя: ");
                        username = Console.ReadLine();
                    }

                    Console.WriteLine("\n------------------------------------------\n");

                    while (password == null)
                    {
                        Console.WriteLine("Введите пароль: ");
                        password = Console.ReadLine();
                    }

                    Console.WriteLine("\n------------------------------------------\n");

                    var result = await CreateUserAsync(username, password);
                    if (result)
                        Console.WriteLine($"Пользователь с ником {username} успешно создан!");
                    else
                        Console.WriteLine("Не удалось создать пользователя");
                }
                if (request == "3")
                {
                    if (_users == null)
                    {
                        Console.WriteLine("Не получен список пользователей!");
                        Console.ReadKey();
                        break;
                    }

                    Console.WriteLine("Выберите пользователя для редактирования: ");
                    int count = 0;
                    foreach (var user in _users)                  
                        Console.WriteLine($"{++count}) {user.Username} | {user.Password}");

                    Console.WriteLine("\n------------------------------------------\n");

                    var parsed = int.TryParse(Console.ReadLine(), out int index);

                    if (parsed && index <= count)
                    {
                        var requestedUser = _users.Skip(index - 1).First();

                        string? newUsername = null;
                        string? newPassword = null;

                        Console.WriteLine("\n------------------------------------------\n");

                        Console.WriteLine("Изменить имя? (y/n)");
                        var changeName = Console.ReadLine();
                        if (changeName == "y")
                        {
                            Console.WriteLine("\n------------------------------------------\n");

                            Console.WriteLine("Введите новое имя: ");
                            newUsername = Console.ReadLine();
                        }

                        Console.WriteLine("\n------------------------------------------\n");

                        Console.WriteLine("Изменить пароль? (y/n)");
                        var changePass = Console.ReadLine();
                        if (changePass == "y")
                        {
                            Console.WriteLine("\n------------------------------------------\n");

                            Console.WriteLine("Введите новый пароль: ");
                            newPassword = Console.ReadLine();
                        }

                        var result = await UpdateUserAsync(
                            requestedUser.Id, 
                            newUsername == null ? requestedUser.Username : newUsername, 
                            newPassword == null ? requestedUser.Password : newPassword
                        );

                        if (result)
                            Console.WriteLine($"Пользователь обновлен!");
                        else
                            Console.WriteLine("Не удалось обновить пользователя");
                    }
                    else if (!parsed)
                    {
                        Console.WriteLine("Некорректный выбор");
                    }
                }
                if (request == "4")
                {
                    if (_users == null)
                    {
                        Console.WriteLine("Не получен список пользователей!");
                        Console.ReadKey();
                        break;
                    }

                    Console.WriteLine("Выберите пользователя для удаления: ");
                    int count = 0;
                    foreach (var user in _users)
                        Console.WriteLine($"{++count}) {user.Username}");

                    Console.WriteLine("\n------------------------------------------\n");

                    var parsed = int.TryParse(Console.ReadLine(), out int index);

                    if (parsed && index <= count)
                    {
                        var requestedUserId = _users.Skip(index - 1).First().Id;

                        Console.WriteLine("\n------------------------------------------\n");

                        var result = await DeleteUserAsync(requestedUserId);
                        if (result)
                            Console.WriteLine($"Пользователь успешно удален!");
                        else
                            Console.WriteLine("Не удалось удалить пользователя");
                    }
                    else if (!parsed)
                    {
                        Console.WriteLine("Некорректный выбор");
                    }
                }

                Console.ReadKey();
            }         
        }

        static async Task<List<User>> GetUsersAsync()
        {
            var reply = await _client.GetUsersAsync(new GetUsersRequest());

            return [.. reply.Users];
        }

        static async Task<bool> CreateUserAsync(string username, string password)
        {
            var reply = await _client.CreateNewUserAsync(new CreateUserRequest { Username = username, Password = password });

            return reply.Result;
        }

        static async Task<bool> UpdateUserAsync(string id, string username, string password)
        {
            var reply = await _client.UpdateUserAsync(new UpdateUserRequest { Id = id, Username = username, Password = password });

            return reply.Result;
        }

        static async Task<bool> DeleteUserAsync(string id)
        {
            var reply = await _client.DeleteUserAsync(new DeleteUserRequest { Id = id });

            return reply.Result;
        }
    }
}
