
using ConsoleAppStateless.Enumerators;
using ConsoleAppStateless.Models;
using Stateless;
using Stateless.Graph;

using var cts = new CancellationTokenSource();
CancellationToken cancellationToken = cts.Token;

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true; // prevent the process from terminating.
    cts.Cancel();
};


var alarm = new Alarm(10, 10, 10, 10);
var input = string.Empty;

WriteHeader();

while (input != "q")
{
    Console.Write("> ");

    input = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(input))
        switch (input.Split(" ")[0])
        {
            case "q":
                Console.WriteLine("Saindo...");
                break;
            case "fire":
                WriteFire(input);
                break;
            case "canfire":
                WriteCanFire();
                break;
            case "state":
                WriteState();
                break;
            case "h":
            case "help":
                WriteHelp();
                break;
            case "c":
            case "clear":
                Console.Clear();
                WriteHeader();
                break;
            default:
                Console.WriteLine("Comando inválido. Digite 'h' ou 'help' para comandos válidos.");
                break;
        }
}



static void WriteHelp()
{
    Console.WriteLine("Valid commands:");
    Console.WriteLine("q               - Sair");
    Console.WriteLine("fire <state>    - Tenta disparar os comandos fornecidos");
    Console.WriteLine("canfire <state> - Retorna uma lista de comandos acionáveis");
    Console.WriteLine("state           - Retorna o estado atual");
    Console.WriteLine("c / clear       - Limpe a janela");
    Console.WriteLine("h / help        - Mostrar isso novamente");
}

void WriteHeader()
{
    Console.WriteLine("Stateless-aplicativo de teste de alarme baseado:");
    Console.WriteLine("---------------------------------------");
    Console.WriteLine("");
}

void WriteCanFire()
{
    foreach (AlarmCommandEnum command in (AlarmCommandEnum[])Enum.GetValues(typeof(AlarmCommandEnum)))
        if (alarm != null && alarm.CanFireCommand(command))
            Console.WriteLine($"{Enum.GetName(typeof(AlarmCommandEnum), command)}");
}

void WriteState()
{
    if (alarm != null)
        Console.WriteLine($"O Currente estado é: {Enum.GetName(typeof(AlarmStateEnum), alarm.CurrentState())}");
}

void WriteFire(string input)
{
    if (input.Split(" ").Length == 2)
    {
        try
        {
            if (Enum.TryParse(input.Split(" ")[1], out AlarmCommandEnum command))
            {
                if (alarm != null)
                    alarm.ExecuteTransition(command);
            }
            else
            {
                Console.WriteLine($"{input.Split(" ")[1]}  AlarmCommand não é válido.");
            }
        }
        catch (InvalidOperationException ex)
        {
            
            Console.WriteLine($"{input.Split(" ")[1]} AlarmCommand não é válido para o estado atual: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Fire requer que você especifique o comando que deseja disparar.");
    }
}