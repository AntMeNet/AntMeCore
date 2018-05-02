using System;
using System.Diagnostics;
using System.Text;

namespace AntMe
{
    /// <summary>
    /// Klasse zum Logging.
    /// </summary>
    public class Tracer
    {
        /// <summary>
        /// Referenz auf die Trace Source.
        /// </summary>
        protected TraceSource Source;

        private readonly bool _autoFlush;

        /// <summary>
        /// Neue Instanz des Tracers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="autoFlush"></param>
        public Tracer(string name, bool autoFlush = true)
        {
            _autoFlush = autoFlush;
            Source = new TraceSource(name);
        }

        /// <summary>
        /// Loggt ein Event.
        /// </summary>
        /// <param name="eventType">Event Type</param>
        /// <param name="id">Event ID</param>
        public void Trace(TraceEventType eventType, int id)
        {
            Source.TraceEvent(eventType, id);
            if (_autoFlush) Source.Flush();
        }

        /// <summary>
        /// Loggt ein Event.
        /// </summary>
        /// <param name="eventType">Event Type</param>
        /// <param name="id">Event ID</param>
        /// <param name="message">Event Message</param>
        public void Trace(TraceEventType eventType, int id, string message)
        {
            Source.TraceEvent(eventType, id, message);
            if (_autoFlush) Source.Flush();
        }

        /// <summary>
        /// Loggt ein Event.
        /// </summary>
        /// <param name="eventType">Event Type</param>
        /// <param name="id">Event ID</param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Trace(TraceEventType eventType, int id, string format, params object[] args)
        {
            Source.TraceEvent(eventType, id, format, args);
            if (_autoFlush) Source.Flush();
        }

        /// <summary>
        /// Loggt ein Event.
        /// </summary>
        /// <param name="eventType">Event Type</param>
        /// <param name="id">Event ID</param>
        /// <param name="message">Event Message</param>
        /// <param name="ex">Dazugehörige Exception</param>
        public void Trace(TraceEventType eventType, int id, string message, Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            string prefix = string.Empty;
            while (ex != null)
            {
                sb.Append(prefix);
                sb.Append(ex.Message);
                sb.Append(prefix);
                sb.Append(ex.StackTrace);
                prefix += " -> ";
                ex = ex.InnerException;
            }

            Source.TraceData(eventType, id, "{0}: {1}", message, sb.ToString());
            if (_autoFlush) Source.Flush();
        }
    }
}
