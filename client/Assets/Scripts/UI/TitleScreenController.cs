using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    private const string TITLE_SCENE_NAME = "Lobbies";

    [SerializeField]
    CanvasGroup playNowButton;

    [SerializeField]
    Image logoImage;

    void Start()
    {
        StartCoroutine(FadeIn(logoImage.GetComponent<CanvasGroup>(), 1f, .1f));
        StartCoroutine(FadeIn(playNowButton, .3f, 1.2f));
    }

    public void PlayNow()
    {
        SceneManager.LoadScene(TITLE_SCENE_NAME);
    }

    IEnumerator FadeIn(CanvasGroup element, float time, float delay)
    {
        yield return new WaitForSeconds(delay);

        for (float i = 0; i <= 1; i += Time.deltaTime / time)
        {
            element.alpha = i;
            yield return null;
        }
    }
}
