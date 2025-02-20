using UnityEngine;

namespace UI.Panels.HUD
{
    public class HUDPanel : UIPanel
    {
        [SerializeField] private HUDInteractibleLabel _interactibleLabel;
        public HUDInteractibleLabel InteractibleLabel =>
            _interactibleLabel;

        private void OnValidate()
        {
            _interactibleLabel = transform.GetComponentInChildren<HUDInteractibleLabel>();
        }
    }
}