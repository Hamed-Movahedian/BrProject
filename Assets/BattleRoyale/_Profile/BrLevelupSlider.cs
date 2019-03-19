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
        var profileManager = ProfileManager.Instance();
        
        Statistics stat = profileManager.PlayerProfile.PlayerStat;
        level = BrExpManager.CalLevel(stat.Experience);
        exp = stat.Experience;
        _prexp = (level > 0) ? BrExpManager.CalXp(level - 1) : 0;
        _nextxp = BrExpManager.CalXp(level);

        Debug.Log(string.Format("Level : {0} \n start XP : {1} \n Current Level : {2} \n Next Level: {3}",level,exp,_prexp,_nextxp));

        float percentXp = (float)(exp - _prexp) / (_nextxp - _prexp);
        ExperienceSlider.fillAmount = percentXp;
        LevelText.text = PersianFixer.Fix((level + 1).ToString(), true, true);
        int newXP = addedXp + exp;
        StartCoroutine(ShowExpChange(newXP));
        stat.Experience = newXP;
        //stat.Level = BrExpManager.CalLevel(newXP);
        profileManager.PlayerProfile.PlayerStat=stat;
        profileManager.SaveProfile();
    }

    IEnumerator ShowExpChange(int Desier)
    {
        float speed = 1;// (Desier - exp)/3;
        showChange:
        Debug.Log(level);
        //ExperienceSlider.fillAmount = 0;

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
                ExperienceSlider.fillAmount = 0;
                goto showChange;
            }
            float amount = (float)(exp - _prexp) / (_nextxp - _prexp);
            
            ExperienceSlider.fillAmount = amount;// / speed;
            yield return null;
            //yield return ChangeSlider(speed,amount);
        }


    }

    private IEnumerator ChangeSlider(float speed, float amount)
    {
        while (ExperienceSlider.fillAmount < amount)
        {
            ExperienceSlider.fillAmount += amount / speed;
            yield return null;
        }
        ExperienceSlider.fillAmount = amount;
        yield return null;
    }
}
