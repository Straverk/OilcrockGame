using UnityEngine;
using UI.Panels;
using System.Collections.Generic;
using System;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

namespace UI
{
    [Serializable, SerializeField]
    public enum UIPanelsTypes
    {
        HUD
    }


    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<UIPanel> panels;

        private Dictionary<UIPanelsTypes, UIPanel> typesUIPanels = new();



        private UIPanelsTypes[] panelTypes =
            Enum.GetValues(typeof(UIPanelsTypes)).OfType<UIPanelsTypes>().ToArray();


        private bool TryAddUIPanel(UIPanelsTypes findPanelType)
        {
            var p = Array.FindAll(gameObject.GetComponentsInChildren<UIPanel>(),
                            x => x.Type == findPanelType);

            if (p.Length == 0)
                Debug.LogErrorFormat("No have {0} UI Panel!", findPanelType);
            else if (p.Length > 1)
                Debug.LogErrorFormat("UI Panel {0} more than need!", findPanelType);
            else
            {
                panels.Add(p[0]);
                return true;
            }

            panels.Add(null);

            return false;
        }


        private void OnValidate()
        {
            panelTypes =
                Enum.GetValues(typeof(UIPanelsTypes)).OfType<UIPanelsTypes>().ToArray();

            if (panels == null || panels[0] == null)
            {
                panels = new();
                foreach (var type in panelTypes)
                    TryAddUIPanel(type);
                return;
            }

            try
            {
                panels.Sort((a, b) => a.Type.CompareTo(b.Type));
            }
            catch (Exception)
            {
                panels = null;

                OnValidate();
                return;
            }

            for (var i = 0; i < panelTypes.Length; i++)
            {
                if (i >= panels.Count)
                {
                    if (!TryAddUIPanel(panelTypes[i]))
                        return;

                    OnValidate();
                    return;
                    //Debug.LogError("UI Panels less than need!");
                    //break;
                }

                if (i == panelTypes.Length - 1 && i != panels.Count - 1)
                {
                    panels.RemoveAt(i + 1);

                    //Debug.LogError("UI Panels more than need!");
                }


                if (panelTypes[i] != panels[i].Type)
                {
                    if (panels[i] != null &&
                        Array.IndexOf(panelTypes, panels[i].Type) >
                        Array.IndexOf(panelTypes, panelTypes[i]))
                    {
                        TryAddUIPanel(panelTypes[i]);

                        OnValidate();
                        return;
                        //Debug.LogErrorFormat("No have {0} Panel!", panelTypes[i]);
                        //break;
                    }
                    else
                    {
                        if (!TryAddUIPanel(panelTypes[i]))
                            return;

                        panels.RemoveAt(i);

                        OnValidate();
                        return;
                        //Debug.LogErrorFormat("{0} Panels find more than one!", panels[i].Type);
                        //break;
                    }
                }
            }
        }



        public void Init()
        {
            for (var i = 0; i < panelTypes.Count(); i++)
            {
                if (panelTypes[i] != panels[i].Type)
                {
                    Debug.LogError("Fix UI errors before sart game!!!");
                    break;
                }
                else
                {
                    typesUIPanels.Add(panelTypes[i], panels[i]);
                    panels[i].Init();
                }
            }
        }

        private UIPanel GetPanelAt(int idx) =>
            GetPanel(panelTypes[idx]);

        public UIPanel GetPanel(UIPanelsTypes panel) =>
            typesUIPanels[panel];

        public void ShowPanel(UIPanelsTypes panel) =>
           GetPanel(panel).Show();
        public void HidePanel(UIPanelsTypes panel) =>
            GetPanel(panel).Hide();

        public void CloseAllPanels()
        {
            for (var i = 0; i < typesUIPanels.Count; i++)
            {
                if (GetPanelAt(i).IsShow)
                    GetPanelAt(i).Hide();
            }
        }

        public void ShowOnePanel(UIPanelsTypes panel)
        {
            CloseAllPanels();
            ShowPanel(panel);
        }
    }
}
