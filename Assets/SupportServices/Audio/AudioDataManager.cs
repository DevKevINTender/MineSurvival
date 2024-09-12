using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IAudioDataManager
{
    public AudioClip GetAudioSOData(AudioEnum audioName);
    public AudioSLData GetAudioSLData();
    public void SetAudioSLData(AudioSLData audioSLData);
}


public class AudioDataManager : IAudioDataManager
{
    [Inject] private IEventService _eventService;
    private const string AudioKey = "AudioKey";
    private ISOStorageService _storageService;
    private AudioSOData _audioSOData;
    private AudioSLData _audioSLData;


    [Inject]
    public void Constructor(ISOStorageService sOStorageService)
    {
        _storageService = sOStorageService;
        _audioSOData = _storageService.GetSOByType<AudioSOData>();
        LoadAudioData();
    }

    public AudioClip GetAudioSOData(AudioEnum audioName) => _audioSOData.audioDictionary[audioName];
    public AudioSLData GetAudioSLData() => _audioSLData;
    public void SetAudioSLData(AudioSLData audioSLData)
    {
        SaveLoader.SaveItem<AudioSLData>(AudioKey, audioSLData);
        _eventService.PublishEvent(new OnChangeAudioValue());
    }

    public void LoadAudioData()
    {
        _audioSLData = new();
        _audioSLData = SaveLoader.LoadItem<AudioSLData>(AudioKey);      
    }
}

public class AudioSLData
{
    public bool MusicValue = true;
    public bool SoundValue = true;
}
