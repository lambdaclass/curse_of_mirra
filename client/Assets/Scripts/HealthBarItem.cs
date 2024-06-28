using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void setYourHelthBar() {
        front.sprite = healthBarGreen;
        damage.sprite = yourLose;
        background.sprite = yourBackground;
    }

    public void setEnemyHelthBar() {
        front.sprite = healthBarRed;
        damage.sprite = enemyLose;
        background.sprite = enemyBackground;
    }

    public void updateHelthBar(float health) {

    }

}
