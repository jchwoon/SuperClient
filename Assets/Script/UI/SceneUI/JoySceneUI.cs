using Data;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoySceneUI : SceneUI
{
    enum GameObjects
    {
        Movestick,
        InteractBtn,
        DashBtn
    }

    enum Images
    {
        InteractImg
    }

    JoyMoveController _joyMoveController;

    Image _interactImg;
    [SerializeField]
    Sprite AttackSprite;
    [SerializeField]
    Sprite InteractSprite;

    public float layoutSkillDist = 200.0f;
    public float[] layoutSkillAngles = new float[4];
    public Transform[] layoutSkillTransform = new Transform[4];
    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GameObject movestick = Get<GameObject>((int)GameObjects.Movestick);
        GameObject interactBtn = Get<GameObject>((int)GameObjects.InteractBtn);
        GameObject dashBtn = Get<GameObject>((int)GameObjects.DashBtn);

        _interactImg = Get<Image>((int)Images.InteractImg);
        _joyMoveController = movestick.GetComponent<JoyMoveController>();

        for (int i = 0; i <  layoutSkillTransform.Length; i++)
        {
            LayoutSkillUi(interactBtn.transform.localPosition, layoutSkillAngles[i], layoutSkillTransform[i]);
        }


        BindEvent(interactBtn, OnInteractBtnClicked);
        BindEvent(movestick, OnMovestickPointerDown, Enums.TouchEvent.PointerDown);
        BindEvent(movestick, OnMovestickPointerUp, Enums.TouchEvent.PointerUp);
        BindEvent(movestick, OnMovestickDrag, Enums.TouchEvent.Drag);
        BindEvent(dashBtn, OnDashBtnClicked);
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        Managers.GameManager.OnInteractableChanged += ChangeInteractBtn;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.GameManager.OnInteractableChanged -= ChangeInteractBtn;
    }

    public void ChangeInteractBtn(IInteractable interactable)
    {
        if (interactable == null)
            _interactImg.sprite = AttackSprite;
        else
        {
            _interactImg.sprite = InteractSprite;
        }
    }

    private void OnMovestickPointerDown(PointerEventData eventData)
    {
        _joyMoveController.OnHandlePointerDown(eventData);
    }

    private void OnMovestickPointerUp(PointerEventData eventData)
    {
        _joyMoveController.OnHandlePointerUp(eventData);
    }

    private void OnMovestickDrag(PointerEventData eventData)
    {
        _joyMoveController.OnHandleDrag(eventData);
    }

    private void OnInteractBtnClicked(PointerEventData eventData)
    {
        if (Managers.GameManager.Interactable == null)
            Managers.EventBus.InvokeEvent(Enums.EventType.AtkBtnClick);
        else
            Managers.GameManager.Interactable.Interact();
    }

    private void OnDashBtnClicked(PointerEventData eventData)
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.DashBtnClick);
    }

    private void LayoutSkillUi(Vector2 pivot, float angle, Transform transform)
    {
        float radian = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * layoutSkillDist;

        transform.localPosition = pivot + offset;
    }
}
