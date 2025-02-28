using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ApplierSlot : BaseUI
{
    enum Texts
    {
        LevelTxt,
        NicknameTxt,
        ClassTxt,
        CombatTxt,
    }
    enum GameObjects
    {
        AcceptBtn,
        RejectBtn,
        InteractionBtns
    }

    public ApplierInfo ApplierInfo { get; private set; }

    GameObject _interactionBtns;

    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _interactionBtns = Get<GameObject>((int)GameObjects.InteractionBtns);

        BindEvent(Get<GameObject>((int)GameObjects.AcceptBtn), OnClickedAcceptBtn);
        BindEvent(Get<GameObject>((int)GameObjects.RejectBtn), OnClickedRejectBtn);
    }

    public void SetInfo(ApplierInfo info)
    {
        TMP_Text levelTxt = Get<TMP_Text>((int)Texts.LevelTxt);
        TMP_Text nicknameTxt = Get<TMP_Text>((int)Texts.NicknameTxt);
        TMP_Text classTxt = Get<TMP_Text>((int)Texts.ClassTxt);
        if (info == null)
        {
            levelTxt.text = string.Empty;
            nicknameTxt.text = string.Empty;
            classTxt.text = string.Empty;
        }
        else
        {
            ApplierInfo = info;
            levelTxt.text = info.Info.Level.ToString();
            nicknameTxt.text = info.Info.Nickname;
            classTxt.text = Utils.GetClassTypeText(info.Info.ClassType);

            Party myParty = Managers.PartyManager.MyParty;
            if (myParty != null && myParty.IsPartyLeader())
            {
                _interactionBtns.SetActive(true);
            }
            else
            {
                _interactionBtns.SetActive(false);
            }
        }
    }

    private void OnClickedAcceptBtn(PointerEventData eventData)
    {

    }

    private void OnClickedRejectBtn(PointerEventData eventData)
    {

    }
}
