using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractSlot : BaseUI
{
    enum Texts
    {
        InteractTxt,
    }

    TMP_Text _interactTxt;
    protected override void Awake()
    {
        base.Awake();
        Bind<TMP_Text>(typeof(Texts));
        _interactTxt = Get<TMP_Text>((int)Texts.InteractTxt);

        //BindEvent(gameObject, )
    }
    public void SetSlot(string text)
    {
        _interactTxt.text = text;
    }

    //해당 NPC가 누구인지를 알아야함
    //NPC클래스에서 Action을 넘겨줘서 여기서 Action을 Invoke하기
    //private void On
}
