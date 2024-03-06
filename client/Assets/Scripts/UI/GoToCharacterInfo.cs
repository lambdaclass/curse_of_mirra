using MoreMountains.Tools;
using UnityEngine;

public class GoToCharacterInfo : MonoBehaviour
{
    public string characterNameString;

    public void GoToCharacterSelection()
    {
        CharactersManager.Instance.SetGoToCharacter(characterNameString);
        GetComponent<MMLoadScene>().LoadScene();
    }
}
