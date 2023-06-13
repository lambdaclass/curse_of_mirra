using UnityEngine;
using MoreMountains.TopDownEngine;

public class InputAoe : MonoBehaviour
{
    GameObject areaWithAim;
    GameObject area;
    GameObject indicator;
    CharacterHandleWeapon _handleWeapon;
    CustomInputManager _inputManager;

    void Start(){
        _handleWeapon = gameObject.GetComponent<CharacterHandleWeapon>();
        _inputManager = gameObject.GetComponent<Character>().LinkedInputManager.GetComponent<CustomInputManager>();
    }

    public void ShowAimAoeAttack()
    {
        //Load the prefab
        areaWithAim = Instantiate(Resources.Load("AreaAim", typeof(GameObject))) as GameObject;
        //Set the prefav as a player child
        areaWithAim.transform.parent = transform;
        //Set its position to the player position
        areaWithAim.transform.position = transform.position;

        //Set scales
        area = areaWithAim.GetComponent<AimHandler>().area;
        area.transform.localScale = area.transform.localScale * 30;
        indicator = areaWithAim.GetComponent<AimHandler>().indicator;
        indicator.transform.localScale = indicator.transform.localScale * 5;
    }
    public void AimAoeAttack(Vector2 aoePosition)
    {
        //Multiply vector values according to the scale of the animation (in this case 12)
        indicator.transform.position = transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
    }
    public void ExecuteAoeAttack(Vector2 aoePosition, Weapon weapon)
    {
        //Destroy attack animation after showing it
        Destroy(areaWithAim, 2.1f);

        indicator.transform.position = transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
        Destroy(indicator, 0.01f);
        Destroy(area, 0.01f);

        _inputManager.customInputPosition = aoePosition;

        _handleWeapon.ChangeWeapon(weapon, weapon.WeaponID, false);
        Debug.Log("Current Weapon: " + _handleWeapon.CurrentWeapon);
        _inputManager.ShootButtonDown();
    }
}
