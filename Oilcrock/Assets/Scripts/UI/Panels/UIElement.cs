using UnityEngine;

namespace UI.Panels
{
    public class UIElement : MonoBehaviour
    {
        public bool IsShow { get; private set; } = true;
        
        public virtual void Show()
        {
            IsShow = true;
            gameObject.SetActive(true);
        }
        
        public virtual void Hide()
        {
            IsShow = false;
            gameObject.SetActive(false);
        }
    }

    public class UIPanel : UIElement
    {
        [SerializeField] protected UIPanelsTypes type;
        public virtual UIPanelsTypes Type { get; private set; }

        public void Init()
        {
            Hide();
        }
    }
}
