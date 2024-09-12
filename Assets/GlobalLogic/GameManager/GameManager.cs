using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private SessionStateMachine _stateMachine;
    [Inject] private IAudioService _audioService;

    [Inject] private GiftServiceManager giftDataManger;

    public void Start()
    {
        DOTween.SetTweensCapacity(2000, 200);
        _stateMachine.SetState<SupportServiceStartState>();
        _audioService.PlayAudio(AudioEnum.BackGroundMusic, true, AudioUnitDataView.AudioType.Music);
/*        giftDataManger.TakeGift(new GiftData() { count = 100, type = GiftEnum.Coin });
        giftDataManger.TakeGift(new GiftData() { count = 25, type = GiftEnum.Gem });
*/    
    }

    private void OnDestroy()
    {
        _stateMachine.SetState<SessionEndState>();
    }
    void OnApplicationQuit()
    {
        //_stateMachine.SetState<SessionEndState>();
    }
}
