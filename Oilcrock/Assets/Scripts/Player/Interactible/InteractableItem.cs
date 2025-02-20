using UnityEngine.Events;
using UnityEngine;

namespace Player.Modules.Interactible
{
    public class InteractableItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactName;
        public string InteractName => interactName;

        [SerializeField] private UnityEvent interactAction;

        public void Interact() =>
            interactAction?.Invoke();
    }
}