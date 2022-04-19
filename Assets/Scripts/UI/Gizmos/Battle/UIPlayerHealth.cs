using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///玩家角色信息
///</summary>
public class UIPlayerHealth : MonoBehaviour
{
    Text levelText;//等级
    Image healthSlider;//当前血量
    Image healthSliderCache;//血量缓存
    Text healthText;//血量信息
    Image expSlider;//当前经验
    CharacterStats playerData;//人物信息
    void Start()
    {
        levelText = transform.GetChild(0).GetComponent<Text>();
        healthSliderCache = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        healthSlider = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        healthText = transform.GetChild(1).GetChild(2).GetComponent<Text>();
        expSlider = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        healthSliderCache.fillAmount = 1;
        healthSlider.fillAmount = 1;
        expSlider.fillAmount = 1;
        if (FightSceneCtrl.Instance != null && FightSceneCtrl.Instance.currentPlayer != null)
            playerData = FightSceneCtrl.Instance.currentPlayer.characterStats;
    }

    void Update()
    {
        if (FightSceneCtrl.Instance != null && FightSceneCtrl.Instance.currentPlayer != null)
        {
            levelText.text = "等级：" + playerData.CurrentLevel;

            healthSlider.fillAmount = (float)playerData.CurrentHealth / playerData.MaxHealth;
            healthText.text = "" + playerData.CurrentHealth + "/" + playerData.MaxHealth;
            if (healthSlider.fillAmount < healthSliderCache.fillAmount)
                healthSliderCache.fillAmount = Mathf.Lerp(healthSliderCache.fillAmount, healthSlider.fillAmount, 0.05f);
            else
                healthSliderCache.fillAmount = healthSlider.fillAmount;

            if (expSlider.fillAmount < (float)playerData.CurrentExp / playerData.LevelUpExp)
                expSlider.fillAmount = Mathf.Lerp(expSlider.fillAmount, (float)playerData.CurrentExp / playerData.LevelUpExp, 0.3f);
            else
                expSlider.fillAmount = (float)playerData.CurrentExp / playerData.LevelUpExp;
        }
    }
}
