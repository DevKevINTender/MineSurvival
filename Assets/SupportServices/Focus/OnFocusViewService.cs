using System.Runtime.InteropServices;
using UnityEngine;

public class OnFocusViewService : MonoBehaviour
{

    private IEventService _eventService;

    private void OnVisibilityChange(string state)
    {
        switch (state)
        {
            case "visible":
                _eventService.PublishEvent(new OnFocus { status = true });
                print("Unity visible");
                break;
            case "hidden":
                _eventService.PublishEvent(new OnFocus { status = false });
                print("Unity hidden");
                break;
        }
    }

    private void Start()
    {
        _eventService = SceneContainerAccessor.SceneContainer.Resolve<IEventService>();

    }

    
}
