using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Reflection;
using System;
using YG;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using static SaveLoader;

public static class SaveLoader
{
    public static T LoadItem<T>(string key) where T : new()
    {
        YandexGame.LoadProgress();
        if (!YandexGame.savesData.myData.ContainsKey(key))
        {
            SaveItem(key, new T());
        }
        return JsonConvert.DeserializeObject<T>(YandexGame.savesData.myData[key]);
    }

    public static T LoadItem<T>(string key, T temp) where T : new()
    {
        YandexGame.LoadProgress();
        if (YandexGame.savesData.myData.ContainsKey(key))
        {
            JsonConvert.PopulateObject(YandexGame.savesData.myData[key], temp);
        }
        
        return temp;
    }

    public static T UpdateItem<T>(string key, T temp) where T : class
    {
        YandexGame.LoadProgress();
        var contractResolver = new CustomContractResolver(); // Пользовательский ContractResolver для игнорирования полей без атрибута JsonSerialize


        if (YandexGame.savesData.myData.ContainsKey(key))
        {
            contractResolver.PopulateObject( temp, JObject.Parse(YandexGame.savesData.myData[key]));
        }

        return temp;
    }

    public static void SaveItem<T>(string key, T item)
    {
        string value = JsonConvert.SerializeObject(item);
        YandexGame.savesData.myData[key] = value;
        YandexGame.SaveProgress();
    }

    public static void ResetAll()
    {
        YandexGame.RemoveLocalSaves();
        YandexGame.ResetSaveProgress();
    }
}

public class CustomContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

        // Оставляем только те свойства, которые помечены атрибутом JsonSerialize
        return properties.Where(p => p.AttributeProvider.GetAttributes(typeof(JsonSerializeAttribute), true).Any()).ToList();
    }

    public void PopulateObject<T>(T target, JObject jObject) where T : class
    {
        var contractResolver = new CustomContractResolver();
        var properties = contractResolver.CreateProperties(typeof(T), MemberSerialization.OptIn);

        foreach (var property in properties)
        {
            var propertyName = property.PropertyName;
            var jsonValue = jObject[propertyName];

            if (jsonValue != null)
            {
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    // Обрабатываем вложенные объекты
                    var existingValue = property.ValueProvider.GetValue(target);
                    if (existingValue == null)
                    {
                        existingValue = Activator.CreateInstance(property.PropertyType);
                        property.ValueProvider.SetValue(target, existingValue);
                    }

                    // Обработка вложенных объектов и массивов
                    if (jsonValue.Type == JTokenType.Object)
                    {
                        PopulateObject(existingValue, (JObject)jsonValue);
                    }
                    else if (jsonValue.Type == JTokenType.Array)
                    {
                        var array = jsonValue.ToObject(property.PropertyType);
                        property.ValueProvider.SetValue(target, array);
                    }
                }
                else
                {
                    // Обрабатываем простые свойства
                    var value = jsonValue.ToObject(property.PropertyType);
                    property.ValueProvider.SetValue(target, value);
                }
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class JsonSerializeAttribute : Attribute
{
}
