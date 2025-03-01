using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager
{
    private int _popupOrder = 10;
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
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        PopupUI popup;
        if (_popups.TryGetValue(name, out popup) == false)
        {
            GameObject go = Managers.ResourceManager.Instantiate(name, Parent);
            popup = go.GetComponent<T>();
            _popups.Add(name, popup);
        }

        popup.gameObject.SetActive(true);
        popup.Canvas.sortingOrder = _popupOrder++;

        return popup as T;
    }

    public T ClosePopupUI<T>(string name = null) where T : PopupUI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T popupUI = Parent.Find(name).gameObject.GetComponent<T>();
        popupUI.gameObject.SetActive(false);

        return popupUI;
    }

    public T GetPopupUI<T>(string name = null) where T : PopupUI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        PopupUI popup;
        if (_popups.TryGetValue(name, out popup) == false)
            return null;

        return popup as T;
    }

    public T ShowSceneUI<T>(string name = null) where T : SceneUI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T sceneUI = Parent.Find(name).gameObject.GetComponent<T>();
        sceneUI.gameObject.SetActive(true);

        return sceneUI;
    }

    public T CloseSceneUI<T>(string name = null) where T : SceneUI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T sceneUI = Parent.Find(name).gameObject.GetComponent<T>();
        sceneUI.gameObject.SetActive(false);

        return sceneUI;
    }

    public T GenerateSlot<T>(Transform parent, string name = null) where T : BaseUI
    {
        if (parent == null)
            return null;

        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.ResourceManager.Instantiate(name, parent);
        return Utils.GetOrAddComponent<T>(go);
    }

    public void ShowFadeUI(float fadeTime = 2.0f, bool isFadeIn = true)
    {
        FadeUI fade = Parent.Find("FadeUI")?.GetComponent<FadeUI>();
        if (fade == null)
            fade = Managers.ResourceManager.Instantiate("FadeUI", Parent).GetComponent<FadeUI>();

        fade.FadeInOut(fadeTime, isFadeIn);
    }

    public void ShowToasUI(string text, float duration = 2)
    {
        ToastUI toast = Parent.Find("ToastUI")?.GetComponent<ToastUI>();
        if (toast == null)
            toast = Managers.ResourceManager.Instantiate("ToastUI", Parent).GetComponent<ToastUI>();

        toast.ShowToast(text, duration);
    }

    public void ShowAlertPopup(string text, Enums.AlertBtnNum btnNum, Action action1 = null, Action action2 = null, Action action3 = null)
    {
        AlertUI alert = Parent.Find("AlertUI")?.GetComponent<AlertUI>();
        if (alert == null)
            alert = Managers.ResourceManager.Instantiate("AlertUI", Parent).GetComponent<AlertUI>();

        alert.gameObject.SetActive(true);
        alert.SetAlert(text, btnNum, action1, action2, action3);

        //return Managers.ResourceManager.Instantiate("Alert", Parent).GetComponent<AlertUI>();
    }

    public CreatureHUD AddCreatureHUD(Creature owner)
    {
        Transform parent = owner.gameObject.transform.Find("HUD");
        GameObject go = Managers.ResourceManager.Instantiate("CreatureHUD", parent);
        CreatureHUD hud = go.GetComponent<CreatureHUD>();

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        canvas.worldCamera = Camera.main;

        return hud;
    }

    public void Clear()
    {
        _popups.Clear();
    }
}
