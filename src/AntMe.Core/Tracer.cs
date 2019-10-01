using System;
using System.Diagnostics;
using System.Text;

namespace AntMe
{
    /// <summary>
    ///     Klasse zum Logging.
    /// </summary>
    public class Tracer
    {
        private readonly bool autoFlush;

        /// <summary>
        ///     Referenz auf die Trace Source.
        /// </summary>
        protected TraceSource source;

        /// <summary>
        ///     Neue Instanz des Tracers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="autoFlush"></param>
        public Tracer(string name, bool autoFlush = true)
        {
            this.autoFlush = autoFlush;
            source = new TraceSource(name);
        }

        /// <summary>
        ///     Loggt ein Event.
        /// </summary>
        /// <param name="eventType">Event Type</param>
        /// <param name="id">Event ID</param>
        public void Trace(TraceEventType eventType, int id)
        {
            source.TraceEvent(eventType, id);
            if (autoFlush) source.Flush();
        }

        /// <summary>
        ///     Loggt ein Event.
        /// </summary>
        /// <param name="eventType">Event Type</param>
        /// <param name="id">Event ID</param>
        /// <param name="message">Event Message</param>
        public void Trace(TraceEventType eventType, int id, string message)
        {
            source.TraceEvent(eventType, id, message);
            if (autoFlush) source.Flush();
        }

        /// <summary>
        ///     Loggt ein Event.
        /// </summary>
        /// <param name="eventType">Event Type</param>
        /// <param name="id">Event ID</param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Trace(TraceEventType eventType, int id, string format, params object[] args)
        {
            source.TraceEvent(eventType, id, format, args);
            if (autoFlush) source.Flush();
        }

        /// <summary>
        ///     Loggt ein Event.
        /// </summary>
        /// <param name="eventType">Event Type</param>
        /// <param name="id">Event ID</param>
        /// <param name="message">Event Message</param>
        /// <param name="ex">Dazugehörige Exception</param>
        public void Trace(TraceEventType eventType, int id, string message, Exception ex)
        {
            var sb = new StringBuilder();

            var prefix = string.Empty;
            while (ex != null)
            {
                sb.Append(prefix);
                sb.Append(ex.Message);
                sb.Append(prefix);
                sb.Append(ex.StackTrace);
                prefix += " -> ";
                ex = ex.InnerException;
            }

            source.TraceData(eventType, id, "{0}: {1}", message, sb.ToString());
            if (autoFlush) source.Flush();
        }
    }
}