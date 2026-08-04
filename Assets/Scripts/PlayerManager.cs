using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(playerInput.playerIndex,new Player(playerInput.gameObject));
        DontDestroyOnLoad(playerInput.gameObject);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        players.Remove(playerInput.playerIndex);
    }

    public void SetJoining(bool canJoin)
    {
        if (canJoin)
        {
            PlayerInputManager.instance.EnableJoining();
        }
        else
        {
            PlayerInputManager.instance.DisableJoining();
        }
    }
}

[System.Serializable]
public struct Player
{
    public GameObject playerObject;
    public Color playerColor;
    public Player(GameObject obj)
    {
        playerObject = obj;
        playerColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
    }
}