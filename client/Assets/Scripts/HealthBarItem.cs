using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using TMPro;

public class HealthBarItem : MonoBehaviour
{
    [SerializeField]
    Sprite healthBarGreen,
        healthBarRed,
        yourLose,
        enemyLose,
        yourBackground,
        enemyBackground;
    
    [SerializeField]
    Image front,
        damage,
        background;

    [SerializeField]
    MMProgressBar healthBarMM;

    [SerializeField]
    GameObject damageText;

    public void setYourHealthBar(ulong health) {
        front.sprite = healthBarGreen;
        damage.sprite = yourLose;
        background.sprite = yourBackground;
        healthBarMM.PercentageTextMeshPro.text = health + "";
        healthBarMM.TextValueMultiplier = health;
    }

    public void setEnemyCrateHealthBar(ulong health) {
        front.sprite = healthBarRed;
        damage.sprite = enemyLose;
        background.sprite = enemyBackground;
        healthBarMM.PercentageTextMeshPro.text = health + "";
        healthBarMM.TextValueMultiplier = health;
    }

    public void updateHealthBar(float health, ulong healthDiff) {
        var damage = Instantiate(damageText, transform.position, Quaternion.identity, transform);
        damage.GetComponent<TextMeshProUGUI>().text = healthDiff + "";
        healthBarMM.BumpOnDecrease = true;
        healthBarMM.TextValueMultiplier = health;
    }
}
