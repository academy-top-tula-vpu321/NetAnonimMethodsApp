
using System.Threading.Channels;

BankAccount account = new(1000);
account.RegisterHandler((message) => Console.WriteLine($"Message from Bank: {message}"));
account.RegisterHandler(ConsoleColorMessage);

account.Take(300);
account.Take(300);

account.UnregisterHandler(ConsoleColorMessage);
account.Take(500);



void ConsoleColorMessage(string message)
{
    var colorOld = Console.ForegroundColor;

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Red message from Bank: {message}");

    Console.ForegroundColor = colorOld;
}

void LambdaExample()
{
    OperInt f1 = delegate (int a, int b) { return a + b; };
    OperInt f2 = (a, b) => a + b;

    Console.WriteLine(f2(60, 70));

    VoidMethod func = delegate (string name)
    {
        Console.WriteLine($"Hello {name}");
    };

    func += delegate (string city)
    {
        Console.WriteLine($"Hello from {city}");
    };

    func("Billy");

    int z = 50;

    int result = Calc(30, 40, (a, b) => a + b);

    //func -= delegate (string city)
    //{
    //    Console.WriteLine($"Hello from {city}");
    //};

    //func?.Invoke("Sammy");

    int Calc(int a, int b, OperInt oper)
    {
        return oper(a, b);
    }

    
}
delegate void VoidMethod(string name);
delegate int OperInt(int a, int b);

public delegate void BankAccountHandler(string message);

class BankAccount
{
    int amount;
    BankAccountHandler? handler;

    public BankAccount(int amount) => this.amount = amount;

    public void Add(int amount) => this.amount += amount;

    public void Take(int amount)
    {
        if (this.amount >= amount)
        {
            this.amount -= amount;
            handler?.Invoke($"С банковского счета снято {amount} рублей");
        }
        else
            handler?.Invoke($"Недостаточно средств для снятия {amount} рублей. " +
                $"Баланс: {this.amount}");
    }

    public void RegisterHandler(BankAccountHandler handler)
    {
        this.handler += handler;
    }

    public void UnregisterHandler(BankAccountHandler handler)
    {
        this.handler -= handler;
    }
}