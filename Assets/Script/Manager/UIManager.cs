using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private int _popupOrder = 10;
    private LinkedList<PopupUI> _popupList = new LinkedList<PopupUI> ();
    private Dictionary<string, PopupUI> _popups = new Dictionary<string, PopupUI>();

    public Transform Parent
    {
        get
        {
            GameObject parentGo = GameObject.Find("UI");
            if (parentGo == null)
                parentGo = new GameObject { name = "UI" };
            return parentGo.transform;
        }
    }

    public T ShowPopup<T>(string name) where T : PopupUI
    {
        if (name == null)
            return null;

        PopupUI popup;
        if (_popups.TryGetValue(name, out popup) == false)
        {
            GameObject go = Managers.ResourceManager.Instantiate(name, Parent);
            popup = go.GetComponent<T>();
            _popups.Add(name, popup);
        }
        _popupList.AddLast(popup);

        popup.gameObject.SetActive(true);
        popup.Canvas.sortingOrder = _popupOrder++;

        return popup as T;
    }

    public T ShowSceneUI<T>(string name) where T : SceneUI
    {
        if (name == null)
            return null;

        T sceneUI = Parent.Find(name).gameObject.GetComponent<T>();
        sceneUI.gameObject.SetActive(true);

        return sceneUI;
    }
}
