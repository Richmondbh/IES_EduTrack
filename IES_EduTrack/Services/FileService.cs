#nullable disable

using System;
using System.IO;
using Newtonsoft.Json;

namespace IES_EduTrack.Services
{
    /// <summary>
    /// This Handles all JSON file persistence for the application.
    /// an it provides generic save and load operations used by all domain services.
    /// </summary>
    public class FileService
    {
        // private constant as our coding guide susgested 
        private const string DefaultFolder = "EduTrackData";

        private readonly string _dataFolderPath;

        
        // Initialises FileService and it ensures the data directory exists.
     
        public FileService()
        {
            // Place the data folder next to the executable so paths are portable
            //I like this implementation because there is always a default folder
            _dataFolderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                DefaultFolder);

            EnsureDataFolderExists();
        }

     
        //Serialises an object to JSON and writes it to the given file name.
        // TypeNameHandling.Auto is required here  for polymorphic types like (Person hierarchy).
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

    
        //Reads a JSON file and deserialises its contents into type T.
        // Returns the type's default value if the file does not exist.
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

       
        // Combines the data folder path with the given file name.
        private string BuildFilePath(string fileName)
        {
            return Path.Combine(_dataFolderPath, fileName);
        }

        // ´this creates the data folder if it does not already exist.
        // and it is Called once during construction.
        private void EnsureDataFolderExists()
        {
            if (!Directory.Exists(_dataFolderPath))
            {
                Directory.CreateDirectory(_dataFolderPath);
            }
        }
    }
}