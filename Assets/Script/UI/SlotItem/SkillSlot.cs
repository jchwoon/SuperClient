using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : BaseUI
{
    enum Images
    {
        SkillImage
    }
    protected override void Awake()
    {
        Bind<Image>(typeof(Images));
    }

    public void SetInfo(SkillData skillData)
    {
        Image image = Get<Image>((int)Images.SkillImage);
        image.sprite = Managers.ResourceManager.GetResource<Sprite>(skillData.IconName);
        image.gameObject.SetActive(true);
    }
}
