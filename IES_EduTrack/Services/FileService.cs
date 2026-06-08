#nullable disable

using System;
using System.IO;
using Newtonsoft.Json;

namespace IES_EduTrack.Services
{
    /// <summary>
    /// Handles all JSON file persistence for the application.
    /// Provides generic save and load operations used by all domain services.
    /// Reused and adapted from CashFlowManager (Assignment 5) and AfriMarket.
    /// </summary>
    public class FileService
    {
        // §1.3f — private constant, not public
        private const string DefaultFolder = "EduTrackData";

        private readonly string _dataFolderPath;

        /// <summary>
        /// Initialises FileService and ensures the data directory exists.
        /// </summary>
        public FileService()
        {
            // Place the data folder next to the executable so paths are portable
            _dataFolderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                DefaultFolder);

            EnsureDataFolderExists();
        }

        /// <summary>
        /// Serialises an object to JSON and writes it to the given file name.
        /// TypeNameHandling.Auto is required for polymorphic types (Person hierarchy).
        /// </summary>
        /// <typeparam name="T">The type of object to serialise.</typeparam>
        /// <param name="fileName">File name, e.g. "students.json".</param>
        /// <param name="data">The object to persist.</param>
        /// <exception cref="IOException">Thrown if the file cannot be written.</exception>
        public void Save<T>(string fileName, T data)
        {
            string filePath = BuildFilePath(fileName);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(data, settings);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Reads a JSON file and deserialises its contents into type T.
        /// Returns the type's default value if the file does not exist.
        /// </summary>
        /// <typeparam name="T">The target deserialisation type.</typeparam>
        /// <param name="fileName">File name, e.g. "students.json".</param>
        /// <returns>Deserialised object, or default(T) if the file is missing.</returns>
        public T Load<T>(string fileName)
        {
            string filePath = BuildFilePath(fileName);

            if (!File.Exists(filePath))
            {
                return default;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// Combines the data folder path with the given file name.
        /// </summary>
        /// <param name="fileName">File name only, no path separators.</param>
        /// <returns>Full absolute path to the file.</returns>
        private string BuildFilePath(string fileName)
        {
            return Path.Combine(_dataFolderPath, fileName);
        }

        /// <summary>
        /// Creates the data folder if it does not already exist.
        /// Called once during construction.
        /// </summary>
        private void EnsureDataFolderExists()
        {
            if (!Directory.Exists(_dataFolderPath))
            {
                Directory.CreateDirectory(_dataFolderPath);
            }
        }
    }
}