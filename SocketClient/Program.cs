// Make the user select a username and a color

using Shared;

Console.Write("Username: ");
string username = Console.ReadLine() ?? string.Empty;
Console.Write("Color: ");
int i = 0;
foreach (object? value in Enum.GetValues(typeof(ConsoleColor)))
    
{
    Console.ForegroundColor = (ConsoleColor) value!;
    Console.WriteLine($"{i++}: {value}");
}
Console.ResetColor();
int j;
do
{
    Console.Write("Input index of wanted color: ");
}
while (!int.TryParse(Console.ReadLine(), out j) || j < 0 || j >= Enum.GetValues(typeof(ConsoleColor)).Length);

ConsoleColor color = (ConsoleColor) Enum.Parse(typeof(ConsoleColor), j+"");
Console.ForegroundColor = color;
Console.WriteLine($"{color} selected");
Console.ReadKey();
Console.ResetColor();
Console.Clear();

new SocketClient.Client("192.168.1.18", new User(color, username));