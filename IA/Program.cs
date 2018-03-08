namespace IA
{
    class Program
    {
        static void Main(string[] args)
        {
            new Client(args[0], int.Parse(args[1])).Start();
        }
    }
}
