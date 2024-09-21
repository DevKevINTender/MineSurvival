using Zenject;
using UnityEngine;
using System;
using System.Collections;
using UniRx;

public class AudioUnitView : PoolingView
{
    public Action DeactivateToPool;

    private AudioSource _audioSource;

    public void Awake()
    {
        gameObject.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
    }

    public void ActivateView(AudioUnitDataView audioUnitDataView)
    {
        gameObject.SetActive(true);
        _audioSource.loop = audioUnitDataView.IsLoopClip;
        _audioSource.clip = audioUnitDataView.Clip;
        _audioSource.Play();

        if (_audioSource.loop == false) StartCoroutine(AudioDelay(_audioSource.clip.length * Time.timeScale));
    }

    private IEnumerator AudioDelay(float audioLenght)
    {
        yield return new WaitForSeconds(audioLenght);
        DeactivateToPool?.Invoke();
    }
    public void DeactivateView()
    {
        _audioSource.loop = false;
        _audioSource.clip = null;
        gameObject.SetActive(false);
    }

    public void ChangeAudioValue(float value)
    {
        _audioSource.volume = value;
    }

    public void ActivateViewFromPool()
    {

    }
}

public class AudioUnitViewService : IPoolingViewService
{
    [Inject] private IEventService _eventService;
    [Inject] private IAudioDataManager _audioDataManager;
    [Inject] private IViewPoolService _viewPoolService;
    private IViewPool _audioUnitViewPool;
    private AudioUnitView _audioUnitView;
    private AudioUnitDataView _audioUnitDataView;
    private Action<IPoolingViewService> _onDeactivateAction;

    public void ActivateService(AudioUnitDataView audioUnitDataView = null)
    {
        _audioUnitDataView = audioUnitDataView;
        _audioUnitView.ActivateView(_audioUnitDataView);
        _eventService.ObserveEvent<OnChangeAudioValue>().Subscribe(ChangeAudioValue);
        ChangeAudioValue();
    }

    private void ChangeAudioValue(OnChangeAudioValue onChangeAudioValue = null)
    {
        AudioSLData audioSLData = _audioDataManager.GetAudioSLData();
        switch(_audioUnitDataView.Type)
        {
            case AudioUnitDataView.AudioType.Music:
                _audioUnitView.ChangeAudioValue(audioSLData.MusicValue ? 1 : 0);
                break;
            case AudioUnitDataView.AudioType.Sound:
                _audioUnitView.ChangeAudioValue(audioSLData.SoundValue ? 1 : 0);
                break;
        }
        
    }

    public void DeactivateServiceToPool()
    {
        _audioUnitView.DeactivateView();
        _onDeactivateAction?.Invoke(this);
    }

    public void SetDeactivateAction(Action<IPoolingViewService> action)
    {
        _onDeactivateAction = action;
    }

    public void ActivateServiceFromPool(Action<IPoolingViewService> action)
    {
        if (_audioUnitView == null)
        {
            _audioUnitViewPool = _viewPoolService.GetPool<AudioUnitView>();
            _audioUnitView = (AudioUnitView)_audioUnitViewPool.GetItem();
            _audioUnitView.DeactivateToPool = DeactivateServiceToPool;
        }
    }
}

public class AudioUnitDataView
{
    public bool IsLoopClip;
    public AudioClip Clip;
    public AudioType Type;

    public AudioUnitDataView(bool isLoopClip, AudioClip clip, AudioType type)
    {
        IsLoopClip = isLoopClip;
        Clip = clip;
        Type = type;
    }

    public enum AudioType
    {
        Music,
        Sound
    }
}
