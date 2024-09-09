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
}
