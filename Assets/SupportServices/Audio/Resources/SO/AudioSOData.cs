using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSOData", menuName = "ScriptableObjects/AudioSOData", order = 1)]
public class AudioSOData : ScriptableObject
{
    [SerializedDictionary]
    public SerializedDictionary<AudioEnum, AudioClip> audioDictionary = new SerializedDictionary<AudioEnum, AudioClip>();

    // Метод для заполнения словаря звуков
    [ContextMenu("FillAudioDictionary")]
    public void FillAudioDictionary()
    {
        // Очистить словарь перед заполнением
        audioDictionary.Clear();

        // Загрузить все аудиофайлы из папки Resources/Audio
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Audio");

        // Перебрать все аудиофайлы и добавить их в словарь
        foreach (AudioClip clip in audioClips)
        {
            // Получить имя файла без расширения
            string fileName = Path.GetFileNameWithoutExtension(clip.name);

            // Преобразовать имя файла в AudioEnum
            AudioEnum audioEnum;
            if (System.Enum.TryParse(fileName, out audioEnum))
            {
                // Добавить звук в словарь
                audioDictionary.Add(audioEnum, clip);
            }
            else
            {
                Debug.LogWarning("Failed to parse audio file: " + fileName);
            }
        }
    }
    [ContextMenu("UpdateAudioEnum")]
    private void UpdateAudioEnum()
    {

        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Audio");
        // Извлекаем имена файлов без расширения
        List<string> audioNames = new List<string>();
        foreach (AudioClip clip in audioClips)
        {
            string fileName = Path.GetFileNameWithoutExtension(clip.name);
            audioNames.Add(fileName);
        }

        // Проверяем текущий enum
        string enumPath = FindEnumFile("AudioEnum");
        string[] existingEnumLines = File.Exists(enumPath) ? File.ReadAllLines(enumPath) : new string[0];

        // Создаем словарь для хранения старых элементов enum
        Dictionary<string, bool> existingEnumElements = new Dictionary<string, bool>();
        foreach (string line in existingEnumLines)
        {
            if (line.Contains("public enum AudioEnum"))
                continue;

            string elementName = line.Trim().TrimEnd(',');
            existingEnumElements[elementName] = true;
        }

        // Генерируем код enum
        string enumCode = "public enum AudioEnum\n{\n";
        foreach (string audioName in audioNames)
        {
            enumCode += $"\t{audioName},\n";
        }
        enumCode += "}\n";

        // Записываем код enum в файл
        File.WriteAllText(enumPath, enumCode);

        // Обновляем редактор, чтобы увидеть новый файл в проекте
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    private string FindEnumFile(string enumName)
    {
        string[] files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

        foreach (string filePath in files)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (line.Contains("enum " + enumName))
                    {
                        return filePath;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading file: " + e.Message);
            }
        }

        return null;
    }
}