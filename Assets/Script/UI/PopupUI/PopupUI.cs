using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : BaseUI
{
    public Canvas Canvas { get; set; }
    protected override void Awake()
    {
        base.Awake();

        Canvas = GetComponent<Canvas>();
    }

    protected void ClosePopup<T>() where T : PopupUI
    {
        Managers.UIManager.ClosePopupUI<T>();
    }
}
