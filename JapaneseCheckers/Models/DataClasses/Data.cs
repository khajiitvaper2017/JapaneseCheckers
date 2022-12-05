using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JapaneseCheckers.Models.DataClasses;

public class Data<T> : IDisposable
{
    private const string path = "Data/";

    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        IncludeFields = false
    };

    public Data()
    {
        Collection = Load();
    }

    public ObservableCollection<T> Collection { get; set; }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Data()
    {
        ReleaseUnmanagedResources();
    }

    protected void Save()
    {
        try
        {

            var name = GetType().Name;
            Directory.CreateDirectory(path);
            using var fs = new FileStream($"{path}{name}.json", FileMode.OpenOrCreate);
            JsonSerializer.Serialize(fs, Collection, options);
        }
        catch (Exception)
        {
            //
        }
    }

    protected ObservableCollection<T> Load()
    {
        try
        {
            var name = GetType().Name;
            using var fs = new FileStream($"{path}{name}.json", FileMode.OpenOrCreate);
            return JsonSerializer.Deserialize<ObservableCollection<T>>(fs, options)!;
        }
        catch
        {
            return new ObservableCollection<T>();
        }
    }

    protected void ReleaseUnmanagedResources()
    {
        Save();
        Collection.Clear();
    }
}