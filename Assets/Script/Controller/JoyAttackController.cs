using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyAttackController : MonoBehaviour
{
    private Image _atkBtnImg;
    private Image _storeBtnImg;
    private bool _isNearNPC = false;

    private void Awake()
    {
        if (transform.childCount > 1)
        {
            Transform firstChild = transform.GetChild(0);
            Transform secondChild = transform.GetChild(1);

            _atkBtnImg = firstChild.GetComponent<Image>();
            _storeBtnImg = secondChild.GetComponent<Image>();
        }
    }
    private void OnEnable()
    {
        Managers.EventBus.AddEvent(Enums.EventType.PlayerNearNPC, () => SetNearNPC(true));
        Managers.EventBus.AddEvent(Enums.EventType.PlayerLeaveNPC, () => SetNearNPC(false));
    }

    private void OnDisable()
    {
        Managers.EventBus.RemoveEvent(Enums.EventType.PlayerNearNPC, () => SetNearNPC(true));
        Managers.EventBus.RemoveEvent(Enums.EventType.PlayerLeaveNPC, () => SetNearNPC(false));
    }

    public void OnHandlePointerClick(PointerEventData eventData)
    {
        if (_isNearNPC)
        {
            //Managers.EventBus.InvokeEvent(Enums.EventType.OpenStore);
            StoreUI storeUI = Managers.UIManager.ShowPopup<StoreUI>();
            storeUI.Refresh();
        }
        else
        {
            Managers.EventBus.InvokeEvent(Enums.EventType.AtkBtnClick);
        }
    }
    public void SetNearNPC(bool isNear)
    {
        _isNearNPC = isNear;

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (_isNearNPC)
        {
            _storeBtnImg.gameObject.SetActive(true);
            _atkBtnImg.gameObject.SetActive(false);
        }
        else
        {
            _storeBtnImg.gameObject.SetActive(false);
            _atkBtnImg.gameObject.SetActive(true);
        }
    }

}
