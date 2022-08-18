using System.Diagnostics;

namespace ClienteMovimiento.ManejoExcepciones
{
    public class ClsExcepcionCapturada
    {
        public static void EscribirEvento(string mensaje)
        {
            EventLog log = new EventLog();
          

            log.Log = "Application";
            if (!EventLog.SourceExists("Application"))
                EventLog.CreateEventSource("Application", "Application");
            log.Source = "Application";
            log.WriteEntry(mensaje,EventLogEntryType.Error);        
        }
    }
}
