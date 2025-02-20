using UnityEngine.Events;
using UnityEngine;

namespace Player.Modules.Interactible
{
    public class TakeableItem : MonoBehaviour, ITakeable
    {
        [SerializeField] private string intemName;
        public string InteractName => intemName;

        [SerializeField] private UnityEvent interactAction;
        [SerializeField] private UnityEvent primaryAction;
        [SerializeField] private UnityEvent secondaryAction;

        public void Interact() =>
            interactAction?.Invoke();

        public void PrimaryAction() => primaryAction?.Invoke();
        public void SecondaryAction() => secondaryAction?.Invoke();
    }
}