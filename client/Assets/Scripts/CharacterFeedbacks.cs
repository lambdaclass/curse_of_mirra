using System.Collections;
using System.Collections.Generic;
using CandyCoded.HapticFeedback;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.VFX;

public enum HapticFeedbackType
{
    Light,
    Heavy
}

public class CharacterFeedbacks : MonoBehaviour
{
    [Header("Setup")]
    public GameObject characterModel;
    public GameObject weaponModel;

    [Header("Feedbacks")]
    [SerializeField]
    List<GameObject> feedbacksStatesPrefabs;

    [SerializeField]
    GameObject deathFeedback,
        damageFeedback,
        healFeedback,
        hitFeedback,
        pickUpFeedback,
        useItemFeedback;


    [SerializeField] 
        GameObject
            goldenClockVFX,
            magicBootsVFX,
            myrrasBlessingVFX;

    [SerializeField]
    MMProgressBar healthBar;

    [SerializeField]
    Color damageOverlayColor;

    [SerializeField]
    Color healOverlayColor;

    Color baseOverlayColor;
    Color currentOverlayColor;
    float overlayTime = 0;
    float overlayDuration = 5f;
    bool restoreBaseOverlayColor = true;
    private bool didPickUp = false;



    // didPickUp value should ideally come from backend
    public bool DidPickUp()
    {
        return didPickUp;
    }

    void Start()
    {
        damageOverlayColor = new Color(255, 0, 0, 1);
        baseOverlayColor = new Color(255, 255, 255, 1);
        healOverlayColor = new Color(68, 173, 68, 1);
    }

    void Update()
    {
        if (restoreBaseOverlayColor && !currentOverlayColor.Equals(baseOverlayColor))
        {
            if (overlayTime < 1)
            {
                overlayTime += (Time.deltaTime / overlayDuration);
            }
            Color32 nextColor = Color.Lerp(currentOverlayColor, baseOverlayColor, overlayTime);

            ChangeModelsOverlayColor(currentOverlayColor);
            currentOverlayColor = nextColor;
        }
    }

    public void SetColorOverlayAlpha(float currentAlpha)
    {
        damageOverlayColor.a = currentAlpha;
        currentOverlayColor.a = currentAlpha;
        baseOverlayColor.a = currentAlpha;
    }
    public void SetActiveStateFeedback(string name, bool active)
    {
        GameObject feedbackToActivate = feedbacksStatesPrefabs.Find(el => el.name == name);
        feedbackToActivate?.SetActive(active);
    }

    public List<GameObject> GetFeedbackStateList()
    {
        return feedbacksStatesPrefabs;
    }

    public void ExecuteFeedback(GameObject feedback)
    {
        feedback.SetActive(true);
    }

    public void PlayDeathFeedback()
    {
        if (characterModel.activeSelf == true)
        {
            deathFeedback.SetActive(true);
        }
    }
    
    private KeyValuePair<float,GameObject> SelectGO(string name){
          switch (name)
        {
            case "mirra_blessing":
                return new KeyValuePair<float,GameObject>(1,myrrasBlessingVFX);
            case "magic_boots":
                return new KeyValuePair<float,GameObject>(10,magicBootsVFX);
            case "golden_clock":
                return new KeyValuePair<float,GameObject>(7,goldenClockVFX);
            default:
                return new KeyValuePair<float,GameObject>(0,null);
        }
    }

    public void ExecuteUseItemFeedback(bool state, string name)
    {
        var obj = SelectGO(name);
        if(obj.Value != null && state == true){
           var vfx = Instantiate(obj.Value, transform);
           vfx.GetComponent<PinnedEffectsController>()?.Setup(this.GetComponent<PinnedEffectsManager>());
           StartCoroutine(this.GetComponent<CharacterMaterialManager>()
            .ResetFresnelTobBase(obj.Key, vfx,  vfx.GetComponent<PinnedEffectsController>()));
        }
    }

    public void ExecutePickUpItemFeedback(bool state)
    {
        didPickUp = state;
        pickUpFeedback.SetActive(state);
    }

    public void DamageFeedback(float clientHealth, float serverPlayerHealth, ulong playerId)
    {
        if (serverPlayerHealth < clientHealth)
        {
            if (playerId == GameServerConnectionManager.Instance.playerId)
            {
                damageFeedback.GetComponent<MMF_Player>().PlayFeedbacks();
                TriggerHapticFeedback(HapticFeedbackType.Heavy);
            }
            this.ChangePlayerTextureOnDamage(clientHealth, serverPlayerHealth);
            this.healthBar.BumpOnDecrease = true;
        }
        if (clientHealth < serverPlayerHealth)
        {
            healFeedback.GetComponent<ParticleSystem>().Play();
            if (playerId == GameServerConnectionManager.Instance.playerId)
            {
                TriggerHapticFeedback(HapticFeedbackType.Light);
            }
        }
    }

    public void ChangePlayerTextureOnDamage(float clientHealth, float playerHealth)
    {
        if (clientHealth != playerHealth)
        {
            if (playerHealth < clientHealth)
            {
                ApplyColorFeedback(damageOverlayColor);
            }
            if (playerHealth > clientHealth)
            {
                ApplyColorFeedback(healOverlayColor);
            }
        }
    }

    public void TriggerHapticFeedback(HapticFeedbackType hapticType)
    {
        switch (hapticType)
        {
            case HapticFeedbackType.Heavy:
                HapticFeedback.HeavyFeedback();
                break;
            case HapticFeedbackType.Light:
                HapticFeedback.LightFeedback();
                break;
        }
    }

    public void ApplyZoneDamage()
    {
        ChangeModelsOverlayColor(damageOverlayColor);
        currentOverlayColor = damageOverlayColor;
    }

    private void ApplyColorFeedback(Color32 color)
    {
        ChangeModelsOverlayColor(color);
        currentOverlayColor = color;
        ResetOverlay();
    }

    public void ResetOverlay()
    {
        overlayTime = 0f;
        restoreBaseOverlayColor = false;
        StartCoroutine(RemoveModelFeedback());
    }

    public void ChangeModelsOverlayColor(Color32 color)
    {
        SkinnedMeshRenderer[] skinnedMeshFilter =
            characterModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var meshFilter in skinnedMeshFilter)
        {
            foreach (var material in meshFilter.materials)
            {
                material.color = color;
            }
        }
    }

    IEnumerator RemoveModelFeedback()
    {
        yield return new WaitForSeconds(.1f);
        restoreBaseOverlayColor = true;
    }

    public void SetActiveFeedback(GameObject player, string feedbackName, bool value)
    {
        SetActiveStateFeedback(feedbackName, value);
    }

    public void ClearAllFeedbacks(GameObject player)
    {
        GetFeedbackStateList().ForEach(el => el.SetActive(false));
    }

    public void PlayHitFeedback()
    {
        hitFeedback.GetComponent<MMF_Player>().PlayFeedbacks();
    }
}
