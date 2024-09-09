using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbySceneUI : SceneUI
{
    enum GameObjects
    {
        CharacterSlotLayout,
    }
    enum Buttons
    {
        BackBtn,
        CreateBtn,
        StartBtn,
        DeleteBtn
    }
    enum Texts
    {
        CurrentSlotNumTxt,
    }

    [SerializeField]
    AlertUI _alertPopup;
    static readonly int MAX_SLOT_COUNT = 5;
    List<LobbyHeroInfo> _heroInfos = new List<LobbyHeroInfo>(MAX_SLOT_COUNT);
    List<GameObject> _slots = new List<GameObject>(MAX_SLOT_COUNT);
    TMP_Text _currentCharacterSlotTxt;
    GameObject _characterSlotLayout;
    GameObject _selectedSlot;
    int _selectSlotIdx;


    float _slotSpacing;
    protected override void Awake()
    {
        base.Awake();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _currentCharacterSlotTxt = Get<TMP_Text>((int)Texts.CurrentSlotNumTxt);
        _characterSlotLayout = Get<GameObject>((int)GameObjects.CharacterSlotLayout);
        _slotSpacing = _characterSlotLayout.GetComponent<HorizontalLayoutGroup>().spacing;

        BindEvent(Get<Button>((int)Buttons.BackBtn).gameObject, OnBackBtnClicked);
        BindEvent(Get<Button>((int)Buttons.CreateBtn).gameObject, OnCreateBtnClicked);
        BindEvent(Get<Button>((int)Buttons.DeleteBtn).gameObject, OnDeleteBtnClicked);
        BindEvent(Get<Button>((int)Buttons.StartBtn).gameObject, OnStartBtnClicked);

        PreCreateSlot();
    }

    public void PreCreateSlot()
    {
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            GameObject go = Managers.ResourceManager.Instantiate("CharacterSlot", _characterSlotLayout.transform);
            go.name = $"Slot{i + 1}";
            go.SetActive(false);
            _slots.Add(go);
        }
    }
    //서버에서 받아온 LobbyHero 정보 세팅
    public void SetCharacterSlotInfo(List<LobbyHeroInfo> heroInfos)
    {
        _heroInfos = heroInfos;
        Refresh();
    }

    private void Refresh()
    {
        //slot 업데이트
        Clear();
        _currentCharacterSlotTxt.text = _heroInfos.Count.ToString();
        float slotSize = 0;
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            if (i < _heroInfos.Count)
            {
                _slots[i].GetComponent<CharacterSlot>().SetSlotInfo(_heroInfos[i], ChangeSelectSlot, i);
                _slots[i].SetActive(true);
                slotSize += _slots[i].GetComponent<RectTransform>().rect.width;
            }
            else
            {
                _slots[i].SetActive(false);
            }
        }
        //레이아웃 정렬
        if (_heroInfos.Count == 1)
            slotSize -= _slotSpacing;
        else
            slotSize += (_slotSpacing * (_heroInfos.Count - 2));
        RectTransform layoutRect = _characterSlotLayout.GetComponent<RectTransform>();
        layoutRect.localPosition = new Vector3(-(slotSize / 2), layoutRect.localPosition.y, layoutRect.localPosition.z);
    }
    private void ChangeSelectSlot(GameObject newSlot, int idx)
    {
        if (newSlot == _selectedSlot)
            return;

        _selectedSlot?.transform.Find("SelectEffect").gameObject.SetActive(false);
        newSlot?.transform.Find("SelectEffect").gameObject.SetActive(true);
        _selectedSlot = newSlot;
        _selectSlotIdx = idx;
    }

    private void OnBackBtnClicked(PointerEventData eventData)
    {
        Clear();
        Managers.SceneManagerEx.ChangeScene(Enums.SceneType.Login);
        Managers.NetworkManager.Disconnect();
    }

    //캐릭터 생성 창 띄우기
    private void OnCreateBtnClicked(PointerEventData eventData)
    {
        if (MAX_SLOT_COUNT <= _heroInfos.Count)
        {
            _alertPopup.gameObject.SetActive(true);
            _alertPopup.SetAlert("더 이상 영웅을 생성할 수 없습니다.", Enums.AlertBtnNum.One);
            return;
        }

        gameObject.SetActive(false);
        Managers.UIManager.ShowSceneUI<CreateHeroSceneUI>();
        Clear();
    }

    private void OnDeleteBtnClicked(PointerEventData eventData)
    {
        if (_selectedSlot == null)
        {
            _alertPopup.gameObject.SetActive(true);
            _alertPopup.GetComponent<AlertUI>().SetAlert("선택된 영웅이 없습니다.", Enums.AlertBtnNum.One);
            return;
        }

        _alertPopup.gameObject.SetActive(true);
        _alertPopup.SetAlert("정말로 삭제하시겠습니까?", Enums.AlertBtnNum.Two, () =>
        {
            ReqDeleteHeroToS reqDeleteHeroPacket = new ReqDeleteHeroToS();
            reqDeleteHeroPacket.HeroIdx = _selectSlotIdx;
            Managers.NetworkManager.Send(reqDeleteHeroPacket);
        });

    }

    private void OnStartBtnClicked(PointerEventData eventData)
    {
        if (_selectedSlot == null)
        {
            _alertPopup.gameObject.SetActive(true);
            _alertPopup.GetComponent<AlertUI>().SetAlert("선택된 영웅이 없습니다.", Enums.AlertBtnNum.One);
            return;
        }
        Managers.GameManager.SelectHeroIdx = _selectSlotIdx;
        Managers.SceneManagerEx.ChangeScene(Enums.SceneType.Game);
    }

    private void Clear()
    {
        _selectedSlot?.transform.Find("SelectEffect").gameObject.SetActive(false);
        _selectedSlot = null;
    }

    #region Receive Packet
    public void OnReceiveDeleteHero(ResDeleteHeroToC packet)
    {
        if (packet.IsSuccess == false)
        {
            _alertPopup.SetAlert("삭제에 실패했습니다.", Enums.AlertBtnNum.One);
            _alertPopup.gameObject.SetActive(true);
            return;
        }
        // slot delete
        _heroInfos.Remove(_heroInfos[packet.HeroIdx]);
        Refresh();
    }
    #endregion
}
