using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AntMe
{
    /// <summary>
    /// Basisklasse einer Kampagne. 
    /// </summary>
    public abstract class Campaign
    {
        /// <summary>
        /// Liefert eine eindeutige GUID für diese Kampagne zurück.
        /// </summary>
        public abstract Guid Guid { get; }

        /// <summary>
        /// Liefert den Namen der Kampagne zurück.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Liefert eine Beschreibung der Kampagne.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Liefert das Kampagnen-Bild im PNG Format, 200x250px oder null, falls der Platzhalter verwendet werden soll.
        /// </summary>
        public abstract byte[] Picture { get; }

        private List<Type> _registeredLevels = new List<Type>();
        private List<bool> _lockState = new List<bool>();
        private int counter = 0;

        /// <summary>
        /// Konstruktor der Kampagne.
        /// </summary>
        public Campaign()
        {
            OnInit();
        }

        /// <summary>
        /// Aufruf bei Initialisierung der Kampagne. Hier sollten alle 
        /// existierenden Levels registriert werden. In dieser Methode sollte 
        /// auch das Einstiegslevel freigeschaltet werden.
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// Wird vom System aufgerufen, wenn eines der Kampagnen-Levels vom 
        /// Spieler beendet wurde. Auf Basis dieser Methode sollten weitere 
        /// Levels freigeschaltet werden.
        /// </summary>
        /// <param name="level">Type des Levels</param>
        /// <param name="lastState">Letzter Zustand der Simulation</param>
        protected abstract void OnFinishLevel(Type level, LevelState lastState);

        /// <summary>
        /// Registriert ein Level als Teil der Kampagne.
        /// </summary>
        /// <param name="level">Betroffenes Level</param>
        protected void RegisterLevel(Type level)
        {
            _registeredLevels.Add(level);
            _lockState.Add(false);
        }

        /// <summary>
        /// Schaltet das angegebene Level für den Spieler frei.
        /// </summary>
        /// <param name="level">Freizuschaltendes Level</param>
        protected void UnlockLevel(Type level)
        {
            for (int i = 0; i < _registeredLevels.Count; i++)
                if (_registeredLevels[i] == level)
                    _lockState[i] = true;
        }

        /// <summary>
        /// Sperrt das angegebene Level wieder für den Spieler.
        /// </summary>
        /// <param name="level">Zu sperrendes Level</param>
        protected void LockLevel(Type level)
        {
            for (int i = 0; i < _registeredLevels.Count; i++)
                if (_registeredLevels[i] == level)
                    _lockState[i] = false;
        }

        /// <summary>
        /// Liefert alle freigeschalteten Levels
        /// </summary>
        /// <returns></returns>
        public List<Type> GetUnlockedLevels()
        {
            List<Type> levels = new List<Type>();

            for (int i = 0; i < _registeredLevels.Count; i++)
                if (_lockState[i])
                    levels.Add(_registeredLevels[i]);

            return levels;
        }

        /// <summary>
        /// Gibt die Kampagnen-Settings als Byte-Array zurück oder legt diese fest.
        /// </summary>
        public byte[] Settings
        {
            get 
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, _lockState);

                    byte[] buffer = new byte[stream.Position];
                    stream.Position = 0;
                    stream.Read(buffer, 0, buffer.Length);

                    return buffer;
                }
            }
            set 
            {
                using (MemoryStream stream = new MemoryStream(value))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    _lockState = formatter.Deserialize(stream) as List<bool>;
                }
            }
        }
    }
}
