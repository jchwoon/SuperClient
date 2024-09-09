using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlertUI : BaseUI
{
    enum Texts
    {
        AlertTxt,
        AcceptTxt
    }
    enum Buttons
    {
        Btn1,
        Btn2,
        Btn3
    }
    enum GameObjects
    {
        AlertImg,
        BtnLayout
    }

    Coroutine _shakeCoroutine;
    TMP_Text _alertMessage;
    RectTransform _alertRect;
    GameObject _btnLayout;
    int _btnNum;
    readonly int MAX_BTN_NUM = 3;
    bool _isBinded;

    Button _btn1;
    Button _btn2;
    Button _btn3;

    Action _firstButtonAction;
    Action _secondButtonAction;
    Action _thirdButtonAction;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_isBinded == true)
            return;

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _alertRect = Get<GameObject>((int)GameObjects.AlertImg).GetComponent<RectTransform>();
        _alertMessage = Get<TMP_Text>((int)Texts.AlertTxt);
        _btnLayout = Get<GameObject>((int)GameObjects.BtnLayout);

        _btn1 = Get<Button>((int)Buttons.Btn1);
        _btn2 = Get<Button>((int)Buttons.Btn2);
        _btn3 = Get<Button>((int)Buttons.Btn3);

        BindEvent(gameObject, OnBackgroundClicked);
        BindEvent(_alertRect.gameObject, (PointerEventData eventData) => { eventData.Use(); });

        BindEvent(_btn1.gameObject, OnFirstBtnClicked);
        BindEvent(_btn2.gameObject, OnSecondBtnClicked);
        BindEvent(_btn3.gameObject, OnThirdBtnClicked);

        _isBinded = true;
    }

    public void SetAlert(string message, Enums.AlertBtnNum btnNum, Action firstBtnAction = null, Action secondBtnAction = null, Action thirdBtnAction = null)
    {
        _btnNum = (int)btnNum;
        _alertMessage.text = message;

        _firstButtonAction = firstBtnAction;
        _secondButtonAction = secondBtnAction;
        _thirdButtonAction = thirdBtnAction;

        float slotSize = 0;

        for (int i = 0; i < MAX_BTN_NUM; i++)
        {
            if (_btnNum > i)
            {
                _btnLayout.transform.GetChild(i).gameObject.SetActive(true);
                slotSize += _btn1.gameObject.GetComponent<RectTransform>().rect.width;
            }
            else
                _btnLayout.transform.GetChild(i).gameObject.SetActive(false);
        }

        slotSize += (100 * (_btnNum - 2));
        RectTransform layoutRect = _btnLayout.GetComponent<RectTransform>();
        layoutRect.localPosition = new Vector3(-(slotSize / 2), layoutRect.localPosition.y, layoutRect.localPosition.z);

        switch (btnNum)
        {
            case Enums.AlertBtnNum.One:
                _btn1.gameObject.transform.GetComponentInChildren<TMP_Text>().text = "확인";
                break;
            case Enums.AlertBtnNum.Two:
                _btn1.gameObject.transform.GetComponentInChildren<TMP_Text>().text = "예";
                _btn2.gameObject.transform.GetComponentInChildren<TMP_Text>().text = "아니요";
                break;
            case Enums.AlertBtnNum.Three:
                _btn1.gameObject.transform.GetComponentInChildren<TMP_Text>().text = "예";
                _btn2.gameObject.transform.GetComponentInChildren<TMP_Text>().text = "아니요";
                _btn3.gameObject.transform.GetComponentInChildren<TMP_Text>().text = "취소";
                break;
        }
    }

    private void OnFirstBtnClicked(PointerEventData eventData)
    {
        _firstButtonAction?.Invoke();
        Clear();
    }
    private void OnSecondBtnClicked(PointerEventData eventData)
    {
        _secondButtonAction?.Invoke();
        Clear();
    }
    private void OnThirdBtnClicked(PointerEventData eventData)
    {
        _thirdButtonAction?.Invoke();
        Clear();
    }

    private void Clear()
    {
        _firstButtonAction = null;
        _secondButtonAction = null;
        _thirdButtonAction = null;
        gameObject.SetActive(false);
    }

    private void OnBackgroundClicked(PointerEventData eventData)
    {
        _shakeCoroutine = StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 origin = _alertRect.localPosition;
        float durationTime = 0.5f;
        float elapsedTime = 0.0f;
        while (elapsedTime < durationTime)
        {
            float x = UnityEngine.Random.Range(-10, 10);
            float y = UnityEngine.Random.Range(-10, 10);
            _alertRect.localPosition = new Vector3 (origin.x + x, origin.y + y, origin.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _alertRect.localPosition = origin;

    }
}
