using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject finalSplash;

    [SerializeField]
    TextMeshProUGUI rankingText,
        rankingTextShadow,
        amountOfKillsText,
        defeaterPlayerName;

    [SerializeField]
    GameObject defeatedByContainer,
        characterModelContainer;

    [SerializeField]
    Image defeaterImage;

    private const int WINNER_POS = 1;
    private const int SECOND_PLACE_POS = 2;
    private const string ZONE_ID = "0";
    CustomCharacter player;
    GameObject modelClone;

    Animator modelAnimator;

    void OnEnable()
    {
        ShowRankingDisplay();
        ShowMatchInfo();
    }

    public void SetDeathSplashCharacter()
    {
        player = Utils.GetCharacter(GameServerConnectionManager.Instance.playerId);

        CoMCharacter character = CharactersManager
            .Instance
            .AllCharacters
            .Single(characterSO => characterSO.name.Contains(player.CharacterModel.name));

        if (character)
        {
            GameObject characterModel = character.UIModel;
            modelClone = Instantiate(characterModel, characterModelContainer.transform);
            modelAnimator = modelClone.GetComponentInChildren<Animator>();
        }
    }

    void ShowRankingDisplay()
    {
        var ranking = GetRanking();
        rankingText.text += " # " + ranking.ToString();
        rankingTextShadow.text += " # " + ranking.ToString();
    }

    private int GetRanking()
    {
        bool isWinner = GameServerConnectionManager
            .Instance
            .PlayerIsWinner(GameServerConnectionManager.Instance.playerId);

        // FIXME This is a temporal for the cases where the winner dies simultaneously
        // FIXME with other/s player/s
        if (isWinner)
        {
            return WINNER_POS;
        }
        if (Utils.GetAlivePlayers().Count() == 0)
        {
            return SECOND_PLACE_POS;
        }
        return Utils.GetAlivePlayers().Count() + 1;
    }

    void ShowMatchInfo()
    {
        // Kill count
        amountOfKillsText.text = Utils
            .GetGamePlayer(GameServerConnectionManager.Instance.playerId)
            ?.Player.KillCount
            .ToString();

        // Defeated By
        if (
            player
            && GameServerConnectionManager
                .Instance
                .PlayerIsWinner(GameServerConnectionManager.Instance.playerId)
        )
        {
            defeatedByContainer.SetActive(false);
        }
        else
        {
            MaybeShowDefeaterName();
            // Defeated By Image
            defeaterImage.sprite = GetDefeaterSprite();
        }
    }

    private ulong GetKillCount()
    {
        // var playerId = GameServerConnectionManager.Instance.playerId;
        // var gamePlayer = Utils.GetGamePlayer(playerId);
        // return gamePlayer.KillCount;
        return 77;
    }

    void MaybeShowDefeaterName(){
        if(KillFeedManager.instance.myKillerId.ToString() == ZONE_ID){
            defeaterPlayerName.gameObject.SetActive(false);
        } else {
            defeaterPlayerName.text = GetDefeaterPlayerName();
        }
    }

    private Sprite GetDefeaterSprite()
    {
        if (KillFeedManager.instance.myKillerId.ToString() == ZONE_ID)
        {
            return KillFeedManager.instance.zoneIcon;
        }
        else
        {
            CoMCharacter killerCharacter = CharactersManager
                .Instance
                .AvailableCharacters
                .Single(
                    characterSO =>
                        characterSO
                            .name
                            .Contains(
                                Utils
                                    .GetPlayer(KillFeedManager.instance.myKillerId)
                                    .GetComponent<CustomCharacter>()
                                    .CharacterModel
                                    .name
                            )
                );
            return killerCharacter.UIIcon;
        }
    }

    private string GetDefeaterPlayerName(){
        return Utils.GetGamePlayer(KillFeedManager.instance.myKillerId).Name;
    }

    public void ShowCharacterAnimation()
    {
        if(player){
            bool isWinner = GameServerConnectionManager.Instance.PlayerIsWinner(GameServerConnectionManager.Instance.playerId);
            string animationName = isWinner ? "Victory" : "Defeat";
            if(modelAnimator.parameterCount > 0){
                bool hasAnimationParameter = AnimationHasParameter(animationName);
                HandleAnimation(animationName, hasAnimationParameter);
            }   
        }
    }

    private bool AnimationHasParameter(string parameterName){
        AnimatorControllerParameter param = modelAnimator.parameters.ToList()
            .Find(p => p.name == parameterName);

        return param != null;
    }

    public void HandleAnimation(string animationName, bool hasAnimationParameter){
        if(hasAnimationParameter){
            modelAnimator.SetBool(animationName, true);
        } else {
            modelAnimator.Play(animationName);
        }
    }
}
