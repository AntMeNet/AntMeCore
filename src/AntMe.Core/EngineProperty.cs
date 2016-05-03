using System;

namespace AntMe
{
    /// <summary>
    ///     Basisklasse für Engine Erweiterungen.
    /// </summary>
    public abstract class EngineProperty : Property
    {
        /// <summary>
        /// Referenz auf die aktuelle Engine.
        /// </summary>
        protected readonly Engine Engine;

        // Last ID: 0
        private readonly Tracer tracer = new Tracer("AntMe.EngineExtension");

        /// <summary>
        /// Standard Konstruktor für Extensions.
        /// </summary>
        /// <param name="engine"></param>
        public EngineProperty(Engine engine)
        {
            Engine = engine;
        }

        /// <summary>
        ///     Wird bei der Initialisierung der Engine aufgerufen.
        /// </summary>
        public abstract void Init();

        /// <summary>
        ///     Wird in jeder Update-Runde von der Engine aufgerufen.
        /// </summary>
        public abstract void Update();

        /// <summary>
        ///     Wird von der Engine aufgerufen, wenn ein neues Item eingefügt wird.
        /// </summary>
        /// <param name="item">Neu eingefügtes Item</param>
        public abstract void Insert(Item item);

        /// <summary>
        ///     Wird von der Engine aufgerufen, wenn ein Item aus der Engine entfernt wird.
        /// </summary>
        /// <param name="item">Das entfernte Item</param>
        public abstract void Remove(Item item);
    }
}