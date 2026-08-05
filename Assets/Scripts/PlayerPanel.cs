using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public int playerId;
    public GameObject firstSelected;
    public GameObject characterPreview;
    private PlayerManager playerManager;
    public GameObject readyIndicator;
    public TextMeshProUGUI playerNameText;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
    }

    void Update()
    {
        characterPreview.GetComponent<Image>().color = playerManager.players[playerId].playerColor;
    }

    public void DisconnectPlayer()
    {
        playerManager.DisconnectPlayer(playerId);
        Destroy(gameObject);
    }

    public void RandomizePlayerColor()
    {
        playerManager.RandomizePlayerColor(playerId);
    }

    public void ToggleReady()
    {
        playerManager.players[playerId].isReady = !playerManager.players[playerId].isReady;
        readyIndicator.SetActive(playerManager.players[playerId].isReady);
    }
}
