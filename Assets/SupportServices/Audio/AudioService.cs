using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IAudioService: IService
{
    public AudioUnitViewService PlayAudio(AudioEnum name, bool isLoop, AudioUnitDataView.AudioType type);
    public AudioUnitViewService PlayAudio(AudioEnum name, bool isLoop);
	public AudioUnitViewService PlayAudio(AudioEnum name);
	public void StopAudio(AudioUnitViewService audio);
}

public class AudioService : IAudioService
{
    private IPoolsViewService _poolsViewService;
    private IPoolViewService _audioUnitPoolViewService;
    private IAudioDataManager _audioDataManager;

	private List<AudioUnitViewService> _activeAudioUnit = new();

	[Inject]
	public void Constructor
	(
		IAudioDataManager audioDataManager,
		IPoolsViewService poolsViewService
	)
	{
		_audioDataManager = audioDataManager;
		_poolsViewService = poolsViewService;
	}
	
	public void ActivateService()
	{
		_audioUnitPoolViewService = _poolsViewService.GetPool<AudioUnitViewService>();
    }

    public AudioUnitViewService PlayAudio(AudioEnum name, bool isLoop, AudioUnitDataView.AudioType type)
    {
        AudioUnitViewService audio = (AudioUnitViewService)_audioUnitPoolViewService.GetItem();
        audio.ActivateService(new AudioUnitDataView(isLoop, _audioDataManager.GetAudioSOData(name), type));
        _activeAudioUnit.Add(audio);
        return audio;
    }

    public AudioUnitViewService PlayAudio(AudioEnum name, bool isLoop)
	{
		AudioUnitViewService audio = (AudioUnitViewService)_audioUnitPoolViewService.GetItem();
		audio.ActivateService(new AudioUnitDataView(isLoop, _audioDataManager.GetAudioSOData(name), AudioUnitDataView.AudioType.Sound));
		_activeAudioUnit.Add(audio);
		return audio;
    }

    public AudioUnitViewService PlayAudio(AudioEnum name)
    {
        AudioUnitViewService audio = (AudioUnitViewService)_audioUnitPoolViewService.GetItem();
        audio.ActivateService(new AudioUnitDataView(false, _audioDataManager.GetAudioSOData(name), AudioUnitDataView.AudioType.Sound));
        _activeAudioUnit.Add(audio);
        return audio;
    }

    public void StopAudio(AudioUnitViewService audio)
    {
        _activeAudioUnit.Remove(audio);
		audio.DeactivateServiceToPool();
    }

    public void DeactivateService()
    {
       
    }
}
