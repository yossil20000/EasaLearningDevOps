using LearningQA.Client.Pages;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LearningQA.Server.Infrasructure
{
	public  class DataResourceReader
	{
        private static string DataResource = "DataResource";
        private static string DefaultDirectory = "TestItem";
        private readonly ILogger _logger;
        IWebHostEnvironment WebHostEnvironment;
       public DataResourceReader(ILogger<DataResourceReader> logger , IWebHostEnvironment webHostEnvironment) { _logger = logger; WebHostEnvironment = webHostEnvironment; }
        public  string[] GetAllJsonFiles( string pattern=null, string directory = "TestItems")
        {
            try
            {
                pattern = string.IsNullOrEmpty(pattern) ? "*.json" : pattern;
                //string filename = $@"{System.IO.Directory.GetCurrentDirectory()}\{DataResource}\{directory}";
                string rootPath = WebHostEnvironment.ContentRootPath;
                string filesDirectory = Path.Combine( DataResource, directory);// $@"{System.IO.Directory.GetCurrentDirectory()}\{DataResource}\{directory}";
                _logger.LogDebug($"DataResourceReader: Json path for Files: filesDirectory {filesDirectory} Pattern: {pattern}");
                //var files = System.IO.Directory.GetFiles(filename,pattern);
                var files = WebHostEnvironment.ContentRootFileProvider.GetDirectoryContents(filesDirectory);
                _logger.LogDebug($"DataResourceReader:System.IO.Directory.GetFiles: Return Count: {files.Count()}");

                StringBuilder sb = new StringBuilder();
                List<string> result = new List<string>();
                foreach (var f in files)
                {
                    if (f.IsDirectory)
                        continue;
                    if (Regex.Match(f.Name, pattern).Success)
                    {
                        string fileFullPath = Path.Combine(filesDirectory, f.Name);
                        sb.AppendLine(fileFullPath);
                        result.Add(fileFullPath);
                    }
                }
                return result.ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public  List<T> LoadJson<T>(string file = "TestItems.TestItem.json")
        {
            if(string.IsNullOrEmpty(file))
			{
                file = "TestItems.TestItem.json";
            }
			else
			{
                file = $@"TestItems\{file}"; 
			}
            var thisAssembly = Assembly.GetExecutingAssembly();
            try
			{
                string filename = $@"{System.IO.Directory.GetCurrentDirectory()}\{DataResource}\{file}";
                var json = System.IO.File.ReadAllText(filename);
                //using (StreamReader r = new StreamReader(thisAssembly.GetManifestResourceStream($"LearningQA.Server.DataResource.{file}")))
                {
                    //var json = r.ReadToEnd();
                    JsonSerializerOptions option = new JsonSerializerOptions();
                    option.IncludeFields = true;
                    option.PropertyNameCaseInsensitive = true;
                    var items = JsonSerializer.Deserialize<List<T>>(json,option).ToList();
                    return items;
                }
            }
            catch(Exception ex)
			{
                _logger.LogError(ex.Message);
			}
            return null;
        }
        public  List<T> LoadJsonFullName<T>(string file,BlockingCollection<List<T>> collection = null )
        {
            if (string.IsNullOrEmpty(file))
            {
                return null;
            }
            var thisAssembly = Assembly.GetExecutingAssembly();
            try
            {
                //var json = System.IO.File.ReadAllText(file);
                string json = string.Empty;
               using( var p = WebHostEnvironment.ContentRootFileProvider.GetFileInfo(file).CreateReadStream() )
                {
                    using(var reader = new StreamReader(p))
                    {
                        json = reader.ReadToEnd();
                    }
                };
                //using (StreamReader r = new StreamReader(thisAssembly.GetManifestResourceStream($"LearningQA.Server.DataResource.{file}")))
                {
                    //var json = r.ReadToEnd();
                    JsonSerializerOptions option = new JsonSerializerOptions();
                    option.IncludeFields = true;
                    option.PropertyNameCaseInsensitive = true;
                    var items = JsonSerializer.Deserialize<List<T>>(json, option).ToList();
                    collection?.Add(items);
                    return items;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodInfo.GetCurrentMethod().Name} File: {file} Ex:{Environment.NewLine} {ex.Message}");
            }
            return null;
        }
        public   string LoadImageForDisplay(string file)
        {
            _logger.LogDebug($"LoadImageForDisplay: Start File: {file}");
            string filename = $@"{System.IO.Directory.GetCurrentDirectory()}\{file}";
            _logger.LogDebug($"{MethodInfo.GetCurrentMethod().Name} FullFileName: {filename} ");
            string imageBase64 = "";
            try
            {
                _logger.LogDebug($"LoadImageForDisplay: file name: {filename}");
                byte[] fileContent = null;
                var fileInfo = WebHostEnvironment.ContentRootFileProvider.GetFileInfo(file);
                long byteLength = fileInfo.Length;
                _logger.LogDebug($"LoadImageForDisplay: byteLength: {byteLength}");
                using (var p = fileInfo.CreateReadStream())
                {

                    using (var reader = new BinaryReader(p))
                    {
                       
                         fileContent = reader.ReadBytes((Int32)byteLength);
                        imageBase64 = Convert.ToBase64String(fileContent);
                        _logger.LogDebug($"LoadImageForDisplay: converted stinr: {imageBase64}");
                    }
                };
                //using System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //using System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
                //long byteLength = new System.IO.FileInfo(filename).Length;
                //_logger.LogDebug($"LoadImageForDisplay: byteLength: {byteLength}");
                //fileContent = binaryReader.ReadBytes((Int32)byteLength);
                //imageBase64 = Convert.ToBase64String(fileContent);
                //_logger.LogDebug($"LoadImageForDisplay: converted stinr: {imageBase64}");
            }
            catch(Exception ex)
            {
                _logger.LogError($"{MethodInfo.GetCurrentMethod().Name} File: {filename} Ex:{Environment.NewLine} {ex.Message}");
            }
            finally
            {
                
                
            }
            return imageBase64;
        }
    }
}
