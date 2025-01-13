using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Enum;
using System;

public class GameSettings
{
    #region Skill
    public static int GetSkillSlotById(int slotIndex)
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
    #endregion

    #region Sound
    public static float GetSound(Enums.ESoundsType soundType)
    {
        float ret = PlayerPrefs.GetInt($"Sound_{soundType}", 50) / SoundManager.MAX_VOLUME_VALUE;
        return ret;
    }

    public static void SetSound(Enums.ESoundsType soundType, int volume)
    {
        PlayerPrefs.SetInt($"Sound_{soundType}", volume);
        PlayerPrefs.Save();
    }
    #endregion
}
