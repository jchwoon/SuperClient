using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescSlot : BaseUI
{
    protected override void Awake()
    {
        base.Awake();
    }


    public void SetInfo(string descText)
    {
        gameObject.SetActive(true);
        GetComponent<TMP_Text>().text = descText;
    }
}
