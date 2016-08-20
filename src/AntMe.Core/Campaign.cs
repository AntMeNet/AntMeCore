using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AntMe
{
    /// <summary>
    /// Basis Class of a campaign.
    /// </summary>
    public abstract class Campaign
    {
        /// <summary>
        /// Returns an unique GUID for this campaign.
        /// </summary>
        public abstract Guid Guid { get; }

        /// <summary>
        /// Returns the name of the campaign.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Returns a description of the campaign.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Returns the campaign picture in PNG format, 200x250px or null, if the place holer is used.
        /// </summary>
        public abstract byte[] Picture { get; }

        private List<Type> _registeredLevels = new List<Type>();
        private List<bool> _lockState = new List<bool>();
        private int counter = 0;

        /// <summary>
        /// Constructor of the campaign.
        /// </summary>
        public Campaign()
        {
            OnInit();
        }

        /// <summary>
        /// Request with initialization of the campaign. All existing levels
        /// should be registered here. In this method the start level should 
        /// be unlocked.
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// Called from the system if one of the campaign levels will be
        /// finished by the player. Based on this method more levels 
        /// will be released.
        /// </summary>
        /// <param name="level">Type of the level</param>
        /// <param name="lastState">Last state of the simulation</param>
        protected abstract void OnFinishLevel(Type level, LevelState lastState);

        /// <summary>
        /// Registers a level as a part of the campaign.
        /// </summary>
        /// <param name="level">Affected level</param>
        protected void RegisterLevel(Type level)
        {
            _registeredLevels.Add(level);
            _lockState.Add(false);
        }

        /// <summary>
        /// Unlocks the selected level for the player.
        /// </summary>
        /// <param name="level">Unlocked level</param>
        protected void UnlockLevel(Type level)
        {
            for (int i = 0; i < _registeredLevels.Count; i++)
                if (_registeredLevels[i] == level)
                    _lockState[i] = true;
        }

        /// <summary>
        /// Locks the selected level again for the player.
        /// </summary>
        /// <param name="level">Locked level</param>
        protected void LockLevel(Type level)
        {
            for (int i = 0; i < _registeredLevels.Count; i++)
                if (_registeredLevels[i] == level)
                    _lockState[i] = false;
        }

        /// <summary>
        /// Get all unlocked levels.
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
        /// Returns the campaign settings in a byte array or sets it.
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
