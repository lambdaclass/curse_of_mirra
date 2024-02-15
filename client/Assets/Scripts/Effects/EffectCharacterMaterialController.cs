using System.Collections;
using UnityEngine;

public class EffectCharacterMaterialController : MonoBehaviour
{
    [SerializeField] private Material character_material = null;
    [SerializeField] private float duration = 1.0f;

    private CharacterMaterialManager character_material_manager = null;

    private void Start()
    {
        character_material_manager = Utils.GetPlayer(GameServerConnectionManager.Instance.playerId)?.GetComponent<CharacterMaterialManager>();
        StartCoroutine(switchMaterialForTime());
    }

    private void OnDestroy()
    {
        deinit();
    }

    public void init()
    {
        character_material_manager?.setMaterial(character_material);
    }

    public void deinit()
    {
        character_material_manager?.resetMaterial();
    }

    private IEnumerator switchMaterialForTime()
    {
        init();
        yield return new WaitForSeconds(duration);
        deinit();
    }
}
