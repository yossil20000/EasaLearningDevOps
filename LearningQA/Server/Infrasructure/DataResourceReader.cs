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
using LearningQA.Shared.Entities;
using Castle.Core.Internal;
using Castle.DynamicProxy.Generators.Emitters;
using System.Runtime.Serialization.Formatters.Binary;

namespace LearningQA.Server.Infrasructure
{
	public  class DataResourceReader
	{
        private static string DataResource = "DataResource";
        private static string DefaultDirectory = "TestItems";
        private readonly ILogger _logger;
        IWebHostEnvironment WebHostEnvironment;
       public DataResourceReader(ILogger<DataResourceReader> logger , IWebHostEnvironment webHostEnvironment) { _logger = logger; WebHostEnvironment = webHostEnvironment; }
        public async Task<bool> WriteDataToJsonAsync(List<TestItem<QUestionSql, int>> items , string desintation)
        {
            try
            {

                string rootPath = WebHostEnvironment.ContentRootPath;
                string destinationfile = Path.Combine(DataResource, DefaultDirectory, desintation);

                using (var writer = new FileStream(destinationfile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    if (writer is not null)
                    {
                        //var binaryWriter = new BinaryFormatter();
                        //binaryWriter.Serialize(serializationStream: writer, items);
                        var jsonFormat = JsonSerializer.Serialize(items);
                       using(var textWriter = new StreamWriter(writer))
                        {
                            await textWriter.WriteAsync(jsonFormat);
                        }
                        return await Task.FromResult(true);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return await Task.FromResult(false);
        }
        public async Task<List<TestItem<QUestionSql, int>>> ReadDataFromJsonAsync( string desintation)
        {
            try
            {

                string rootPath = WebHostEnvironment.ContentRootPath;
                string destinationfile = Path.Combine(DataResource, DefaultDirectory, desintation);

                using (var reader = new FileStream(destinationfile, FileMode.Open, FileAccess.Read))
                {
                    if (reader is not null)
                    {
                        //var binaryWriter = new BinaryFormatter();
                        //binaryWriter.Serialize(serializationStream: writer, items);
                        
                        using (var textReader = new StreamReader(reader))
                        {
                            int streamLenght = (int)reader.Length;
                            char[] json = new char[streamLenght];

                            var nextChar = await textReader.ReadAsync(json, 0,streamLenght);
                            var jsonFormat = JsonSerializer.Deserialize<List<TestItem<QUestionSql, int>>>(json);
                            return await Task.FromResult(jsonFormat);
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return await Task.FromResult(new List<TestItem<QUestionSql, int>>());
        }
        public  string[] GetAllJsonFiles( string pattern=null, string directory = "TestItems")
        {
            List<string> result = new List<string>();
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
               
                foreach (var f in files)
                {
                    if (f.IsDirectory)
                        continue;
                    if (Regex.IsMatch(f.Name, pattern,RegexOptions.IgnoreCase))
                    {
                        string fileFullPath = Path.Combine(filesDirectory, f.Name);
                        sb.AppendLine(fileFullPath);
                        result.Add(fileFullPath);
                    }
                }
                return result.ToArray();

            }
            catch(System.ArgumentException ae)
            {
                _logger.LogError(ae.Message);
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
        public  List<T> LoadJsonFullName<T>(string file )
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

        public bool ConverSupplement(List<TestItem<QUestionSql, int>> items)
        {
            try
            {
                //The you can add aditional info that you can put and it latter can be filter, some third party can useit
                //Don't use string interpolation
                _logger.LogTrace(100, "ConverSupplement: start at {time}", DateTime.UtcNow);
                for (int item = 0; item < items.Count; item++)
                {
                    _logger.LogTrace($"ConverSupplement: loop on {items.ElementAt(item).GeTestItemTitle()}");
                    for (int qIndex = 0; qIndex < items.ElementAt(item).Questions.Count; qIndex++)
                    {
                        var toRemove = items.ElementAt(item).Questions.ElementAt(qIndex).Supplements.Where(x => x.OriginalContent.IsNullOrEmpty() && x.Title.IsNullOrEmpty()).ToList();
                        _logger.LogTrace($"ConverSupplement: ToRemove Count:  {toRemove.Count}");
                        var supplement = items.ElementAt(item).Questions.ElementAt(qIndex).Supplements;
                        _logger.LogTrace($"ConverSupplement: Supplements Count:  {supplement.Count}");

                        toRemove.ForEach(item => supplement.Remove(item));
                        for (int suppIndex = 0; suppIndex < supplement.Count; suppIndex++)
                        {
                            _logger.LogTrace($"ConverSupplement: Supplements After remove Count:  {supplement.Count} suppIndex: {suppIndex}");
                            switch (supplement.ElementAt(suppIndex).OriginalcontentType)
                            {
                                case ContentType.ImageFileName:
                                case ContentType.ImageFileNameExplain:
                                    {
                                        _logger.LogTrace($"ConverSupplement: item:{item}, Question:{items.ElementAt(item).Questions.ElementAt(qIndex).QuestionNumber} Supp:{supplement.ElementAt(suppIndex).OriginalContent}");
                                        var content = supplement.ElementAt(suppIndex).OriginalContent.Split(";");
                                        if (content.Length > 0)
                                        {
                                            var src = content[0].Split(":")[1];
                                            var srcString = LoadImageForDisplay(src);
                                            _logger.LogTrace($"ConverSupplement: scrString: {srcString}");
                                            supplement.ElementAt(suppIndex).Content = srcString;
                                            supplement.ElementAt(suppIndex).ContentType = ContentType.ImageBase64String;
                                            _logger.LogTrace($"ConverSupplement: scrString: {supplement.ElementAt(suppIndex).Content}");

                                        }

                                    }
                                    break;
                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return false;
            }

        }

        public  Task<string> LoadAllJsonAsync(string path,out List<TestItem<QUestionSql, int>> result)
        {
            StringBuilder sb = new StringBuilder();
            result = new List<TestItem<QUestionSql, int>>();
            var jsonFiles = GetAllJsonFiles(path);

            if (jsonFiles is null)
                return Task.FromResult(sb.AppendLine("GetAllJsonFiles Return Null").ToString());
            
            try
            {
                foreach (var file in jsonFiles)
                {

                    var testItems = LoadJsonFullName<TestItem<QUestionSql, int>>(file);
                    if (testItems == null)
                        continue;
                    if (testItems.Where(x => string.IsNullOrEmpty(x.Category) || string.IsNullOrEmpty(x.Chapter) || string.IsNullOrEmpty(x.Subject)).Any())
                        continue;
                    if (!ConverSupplement(testItems))
                    {
                        sb.AppendLine($"ConvertSupplement Failed  On : ");
                    }
                    var supp = testItems.SelectMany(x => x.Questions).Where(x => x.Supplements.Count > 0).Select(x => x.Supplements.FirstOrDefault());
                    foreach (var s in supp)
                    {
                        _logger.LogTrace($"Test item supplemnts: {s.OriginalContent} {s.Content}");
                    }

                    result.AddRange(testItems);
                    Debug.WriteLine($"Finished File{file} ");
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return Task.FromResult(sb.ToString());
        }
    }
}
