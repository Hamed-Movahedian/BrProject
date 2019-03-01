using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrLevelupSlider : MonoBehaviour
{
    private int level;
    private int exp;

    public Image ExperienceSlider;
    public Text LevelText;
    private int _prexp;
    private int _nextxp;
    public int addedXp;

    private void OnEnable()
    {
        Statistics stat = ProfileManager.Instance().PlayerProfile.PlayerStat;
        level = stat.Level;
        exp = stat.Experience;
        _prexp = (level > 0) ? BrExpManager.CalXp(level - 1) : 0;
        _nextxp = BrExpManager.CalXp(level);

        float percentXp = (float)(exp - _prexp) / (_nextxp - _prexp);
        ExperienceSlider.fillAmount = percentXp;
        LevelText.text = PersianFixer.Fix((level + 1).ToString(), true, true);
        StartCoroutine(ShowExpChange(addedXp+exp));
    }

    IEnumerator ShowExpChange(int Desier)
    {
        float speed = (Desier - exp)/3;
        showChange:
        Debug.Log(level);
        ExperienceSlider.fillAmount = 0;

        while (exp < Desier)
        {
            exp++;
            if (exp > _nextxp)
            {
                level++;
                _prexp = _nextxp;
                _nextxp = BrExpManager.CalXp(level);
                exp--;
                LevelText.text = PersianFixer.Fix((level + 1).ToString(), true, true);
                goto showChange;
            }
            float amount = (float)(exp - _prexp) / (_nextxp - _prexp);

            yield return ChangeSlider(speed,amount);
        }


    }

    private IEnumerator ChangeSlider(float speed, float amount)
    {
        while (ExperienceSlider.fillAmount<amount)
        {
            ExperienceSlider.fillAmount += amount/speed;
            yield return null;
        }
        ExperienceSlider.fillAmount = amount;
        yield return null;
    }
}
