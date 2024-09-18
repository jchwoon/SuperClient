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

    //�ش� NPC�� ���������� �˾ƾ���
    //NPCŬ�������� Action�� �Ѱ��༭ ���⼭ Action�� Invoke�ϱ�
    //private void On
}
