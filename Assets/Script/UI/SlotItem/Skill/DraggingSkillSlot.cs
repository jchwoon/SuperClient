using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class DraggingSkillSlot : BaseUI
{
    enum Images
    {
        SkillImage
    }

    Image _skillImage;
    RectTransform _slotRect;
    RectTransform _canvasRect;
    protected override void Awake()
    {
        Bind<Image>(typeof(Images));

        _skillImage = Get<Image>((int)Images.SkillImage);
        _slotRect = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(Sprite sprite)
    {
        gameObject.SetActive(true);
        _skillImage.sprite = sprite;
    }

    public void OnDrag(Vector2 screenPos, RectTransform canvasRect)
    {
        Vector2 touchPos = Vector2.zero;
        bool inner = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out touchPos);
        if (inner)
        {
            _slotRect.anchoredPosition = touchPos;
        }
    }

    public void OnEndDrag()
    {
        gameObject.SetActive(false);
        _skillImage.sprite = null;
    }
}
