using System.Collections;
using System.Collections.Generic;
using TMPro;
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



    public T ShowPopup<T>(string name = null) where T : PopupUI
    {
        if (name == null)
            name = typeof(T).Name;

        PopupUI popup;
        if (_popups.TryGetValue(name, out popup) == false)
        {
            GameObject go = Managers.ResourceManager.Instantiate(name, Parent);
            popup = go.GetComponent<T>();
            _popups.Add(name, popup);
        }
        LinkedListNode<PopupUI> node = _popupList.Find(popup);
        if (node == null)
            _popupList.AddLast(popup);
        else
        {
            _popupList.AddLast(popup);
            _popupList.Remove(node);
        }


        popup.gameObject.SetActive(true);
        popup.Canvas.sortingOrder = _popupOrder++;

        return popup as T;
    }

    public T ClosePopupUI<T>(string name = null) where T : PopupUI
    {
        if (name == null)
            name = typeof(T).Name;

        T popupUI = Parent.Find(name).gameObject.GetComponent<T>();
        popupUI.gameObject.SetActive(false);

        return popupUI;
    }

    public T ShowSceneUI<T>(string name = null) where T : SceneUI
    {
        if (name == null)
            name = typeof(T).Name;

        T sceneUI = Parent.Find(name).gameObject.GetComponent<T>();
        sceneUI.gameObject.SetActive(true);

        return sceneUI;
    }

    public T CloseSceneUI<T>(string name = null) where T : SceneUI
    {
        if (name == null)
            name = typeof(T).Name;

        T sceneUI = Parent.Find(name).gameObject.GetComponent<T>();
        sceneUI.gameObject.SetActive(false);

        return sceneUI;
    }

    public AlertUI ShowAlertPopup(Transform parent)
    {
        return Managers.ResourceManager.Instantiate("Alert", parent).GetComponent<AlertUI>();
    }
}
