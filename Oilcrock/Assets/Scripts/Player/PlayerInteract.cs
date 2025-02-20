using UnityEngine;
using GameSettings;
using UI;
using UI.Panels.HUD;
using Player.Modules.Interactible;
using SaveUtils;

namespace Player.Modules
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private ArmInteraction _armInteraction;

        [SerializeField] private LayerMask _interactMask;


        private void OnValidate()
        {
            _cameraTransform ??= transform.GetComponentInChildren<Camera>().transform;
            _armInteraction ??= transform.GetComponentInChildren<ArmInteraction>();

            if (_interactMask == 0)
                Debug.LogError("Create a 'Interactible' layer and set up it to the 'Interact Mask'");
        }



        private PlayerConfig _playerConfig;
        private HUDInteractibleLabel _interactibleLabel;

        private IInteractable _lastInteractible;


        public void Init(PlayerInputSystem inputSystem,
            PlayerConfig playerConfig, UIManager uiManager)
        {
            _playerConfig = playerConfig;



            if (uiManager.GetPanel(UIPanelsTypes.HUD) is HUDPanel hud)
                _interactibleLabel = hud.InteractibleLabel;


            _armInteraction.Init(playerConfig);


            inputSystem.SetInteractHandler(Interact);
        }

        private Component GetRaycastInteractible()
        {
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward,
                out RaycastHit hit, _playerConfig.InteractDistance, _interactMask) &&
                hit.transform.TryGetComponent(typeof(IInteractable), out Component component))
            {
                return component;
            }

            return null;
        }

        private void InteractUpdate()
        {
            Component component = GetRaycastInteractible();
            if (component is IInteractable interactible)
            {
                if (_lastInteractible != interactible)
                {
                    _interactibleLabel.ShowInteract(interactible);
                    _lastInteractible = interactible;
                }
            }
            else if (_lastInteractible != null)
            {
                _interactibleLabel.Hide();
                _lastInteractible = null;
            }
        }

        private void Interact(float input)
        {
            // interact on input equal true
            if (input == 0)
                return;

            Component component = GetRaycastInteractible();

            if (component == null)
                _armInteraction.DropItem();

            if (component is ITakeable takeable)
            {
                _armInteraction.TakeItem(component, takeable);
            }
            else if (component is IInteractable interactable)
            {
                interactable.Interact();
            }
        }

        private void FixedUpdate()
        {
            InteractUpdate();
        }
    }
}