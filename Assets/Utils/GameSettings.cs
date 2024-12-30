using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Enum;
using System;

public class GameSettings
{
    public static int GetSkillSlotTemplateId(int slotIndex)
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return 0;

        return PlayerPrefs.GetInt($"{hero.Name}_SkillSlot_{slotIndex}", 0);
    }

    public static void SetSkillSlot(int slotIndex, int templateId)
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return;

        PlayerPrefs.SetInt($"{hero.Name}_SkillSlot_{slotIndex}", templateId);
        PlayerPrefs.Save();
    }
}
