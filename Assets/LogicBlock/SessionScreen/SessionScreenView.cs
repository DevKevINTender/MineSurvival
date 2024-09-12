using Zenject;
        using UnityEngine;

        public class SessionScreenView : MonoBehaviour
        {
            // Add your fields and methods here
        }

        public class SessionScreenService : IService
        {
            [Inject] private IViewFabric _fabric;
            [Inject] private IMarkerService _markerService;
            private SessionScreenView _SessionScreenView;

            public void ActivateService()
            {       
                _SessionScreenView = _fabric.Init<SessionScreenView>();
            }

            public void DeactivateService()
            {       
                // Add your deactivation logic here
            }
        }