using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JapaneseCheckers.Models;

public class Data<T> : IDisposable
{
    private const string path = "";

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

    private void Save()
    {
        var name = GetType().Name;
        using var fs = new FileStream($"{path}{name}.json", FileMode.OpenOrCreate);
        JsonSerializer.Serialize(fs, Collection,new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            
        });
    }

    private ObservableCollection<T> Load()
    {
        try
        {
            var name = GetType().Name;
            using var fs = new FileStream($"{path}{name}.json", FileMode.OpenOrCreate);
            return JsonSerializer.Deserialize<ObservableCollection<T>>(fs);
        }
        catch
        {
            return new ObservableCollection<T>();
        }
    }

    private void ReleaseUnmanagedResources()
    {
        Save();
        Collection.Clear();
    }
}