using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public TextMeshProUGUI playerCountText;
    public List<Player> players = new List<Player>();

    void Update()
    {
        int playerCount = PlayerInput.all.Count;
        playerCountText.text = "Players: " + playerCount;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(new Player(playerInput.playerIndex, playerInput.gameObject));
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        players.RemoveAll(p => p.playerId == playerInput.playerIndex);
    }
}

[System.Serializable]
public struct Player
{
    public int playerId;
    public GameObject playerObject;
    public Color playerColor;
    public Player(int id, GameObject obj)
    {
        playerId = id;
        playerObject = obj;
        playerColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
    }
}