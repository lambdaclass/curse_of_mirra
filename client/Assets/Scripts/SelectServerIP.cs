using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectServerIP : MonoBehaviour
{
    [SerializeField]
    Text IP;

    [SerializeField]
    GameObject Button;

    [SerializeField]
    Text ButtonText;
    public Color selectedColor = Color.white;

    Image ButtonImage;

    public static string serverIp;

    void Awake()
    {
        ButtonImage = Button.GetComponent<Image>();
    }

    void Update()
    {
        SetButtonContent();
    }

    private void SetButtonContent()
    {
        if (LobbyConnection.Instance.server_ip == IP.text)
        {
            ButtonText.text = "Connected!";
        }
        else
        {
            ButtonText.text = "Connect";
        }
    }

    //This method is called when the button is pressed
    public void SetServerIp()
    {
        serverIp = IP.text;
        LobbyConnection.Instance.Refresh();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static string GetServerIp()
    {
        return string.IsNullOrEmpty(serverIp) ? "localhost" : serverIp;
    }
}
