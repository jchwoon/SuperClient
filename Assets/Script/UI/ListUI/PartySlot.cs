using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartySlot : BaseUI
{

    protected enum Texts
    {
        Member1Name,
        Member2Name,
        Member3Name,
        Member4Name,
        Member1LV,
        Member2LV,
        Member3LV,
        Member4LV,
    }

    TMP_Text _member1Name;
    TMP_Text _member2Name;
    TMP_Text _member3Name;
    TMP_Text _member4Name;
    TMP_Text _member1LV;
    TMP_Text _member2LV;
    TMP_Text _member3LV;
    TMP_Text _member4LV;



    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));


        _member1Name = Get<TMP_Text>((int)Texts.Member1Name);
        _member2Name = Get<TMP_Text>((int)Texts.Member2Name);
        _member3Name = Get<TMP_Text>((int)Texts.Member3Name);
        _member4Name = Get<TMP_Text>((int)Texts.Member4Name);
        _member1LV = Get<TMP_Text>((int)Texts.Member1LV);
        _member2LV = Get<TMP_Text>((int)Texts.Member2LV);
        _member3LV = Get<TMP_Text>((int)Texts.Member3LV);
        _member4LV = Get<TMP_Text>((int)Texts.Member4LV);
    }
    
    public void SetUserName(string userName, int userLv)
    {
        //TODO List로 유저 정보 받아오기.

        string lv = "LV. " + userLv;

        _member1Name.text = userName;
        _member1LV.text = lv;
    }


}
