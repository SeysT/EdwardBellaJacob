using System.Diagnostics;

namespace IA
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener($"Trace_{System.DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss")}.txt"));
            Trace.TraceInformation("Initialisation du client");

            new Client(args[0], int.Parse(args[1])).Start();
        }
    }
}
