using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(StateManager))]
public class PlayerManager : MonoBehaviour
{
    public Dictionary<int, Player> players = new Dictionary<int, Player>();
    public GameObject playerPanelsParent;
    public GameObject playerPanelPrefab;
    private StateManager stateManager;
    public float outOfBoundsTime = 3f;
    public Transform jailPoint;
    public Transform releasePoint;
    [SerializeField] private int playersRequiredToStart = 2;

    private readonly List<Player> jailedPlayers = new();

    public static PlayerManager Instance;

    void Start()
    {
        Instance = this;
        stateManager = GetComponent<StateManager>();
    }

    void Update()
    {
        if (stateManager.isInPlayerSelect)
        {
            if (players.Count <= 0)
            {
                SetJoining(true);
            }
            else
            {
                bool allReady = true;
                foreach (var player in players.Values)
                {
                    if (!player.isReady)
                    {
                        allReady = false;
                        break;
                    }
                }
                if (allReady && players.Count >= playersRequiredToStart)
                {
                    foreach (var player in players.Values)
                    {
                        player.playerObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                    }
                    UnityEngine.SceneManagement.SceneManager.LoadScene(StateManager.GAME_SCENE_INDEX);
                }
            }
        }
        else if (stateManager.isInGame)
        {
            SetJoining(false);

            if (stateManager.isInGame)
            {
                foreach (var player in players.Values)
                {
                    if (!player.isInZone && !player.isInJail)
                    {
                        if (!player.checkedLeftZone)
                        {
                            player.checkedLeftZone = true;
                            player.timeLeftZone = Time.time;
                        }
                        else
                        {
                            if (Time.time - player.timeLeftZone >= outOfBoundsTime)
                            {
                                KillPlayer(player);
                            }
                        }
                    }
                    else if (player.isInZone)
                    {
                        player.checkedLeftZone = false;
                    }
                }
            }

            bool allInJail = true;
            foreach (var player in players.Values)
            {
                if (!player.isInJail)
                {
                    allInJail = false;
                    break;
                }
            }
            if (allInJail && players.Count > 0)
            {
                StateManager.Instance.EndGame(false);
            }
        }
        else if (stateManager.isInEndScreen)
        {

            SetJoining(false);
        }
    }

    private void KillPlayer(Player player)
    {
        StateManager.Instance.hasPlayerDied = true;
        player.isInJail = true;
        player.DropItem();
        player.playerObject.transform.position = jailPoint.position;
        jailedPlayers.Add(player);

        Shop.Instance.RefreshEntries();
        Stats.Instance.PlayersLost++;
    }

    public bool IsPlayerInJail()
    {
        return jailedPlayers.Count > 0;
    }

    public void ReleasePlayer()
    {
        var idx = Random.Range(0, jailedPlayers.Count);

        var p = jailedPlayers[idx];
        jailedPlayers.RemoveAt(idx);

        p.isInJail = false;
        p.playerObject.transform.position = releasePoint.position;

        Shop.Instance.RefreshEntries();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        GameObject playerObject = playerInput.gameObject;
        players.Add(playerInput.playerIndex, new Player(playerObject));
        DontDestroyOnLoad(playerObject);

        GameObject playerPanel = Instantiate(playerPanelPrefab, playerPanelsParent.transform);
        players[playerInput.playerIndex].playerPanel = playerPanel;
        PlayerPanel panelScript = playerPanel.GetComponent<PlayerPanel>();
        panelScript.playerId = playerInput.playerIndex;
        panelScript.playerNameText.text = "Player " + (playerInput.playerIndex + 1);
        MultiplayerEventSystem mes = playerObject.GetComponent<MultiplayerEventSystem>();
        if (mes != null)
        {
            mes.playerRoot = playerPanel;
            mes.firstSelectedGameObject = panelScript.firstSelected;
        }
        // playerObject.GetComponent<InputSystemUIInputModule>().actionsAsset = playerInput.actions;
        playerInput.SwitchCurrentActionMap("UI");
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        players.Remove(playerInput.playerIndex);
    }

    public void DisconnectPlayer(int playerId)
    {
        Destroy(players[playerId].playerObject);
        players.Remove(playerId);
    }

    public void RandomizePlayerColor(int playerId)
    {
        players[playerId].playerColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        players[playerId].playerObject.GetComponent<SpriteRenderer>().color = players[playerId].playerColor;
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

public class Player
{
    public GameObject playerObject;
    public GameObject playerPanel = null;
    public Color playerColor;
    public bool isReady = false;
    public bool isInZone = true;
    public bool isInJail = false;
    public bool checkedLeftZone = false;
    public float timeLeftZone = 0;
    public ItemStackDisplay heldItem;
    public Player(GameObject obj)
    {
        playerObject = obj;
        playerColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
    }

    public void SetItem(ItemStackDisplay item)
    {
        heldItem = item;
        item.transform.SetParent(playerObject.GetComponent<PlayerController>().dockingPoint.transform);
        item.transform.localPosition = Vector3.zero;
        item.gameObject.layer = LayerMask.NameToLayer("Default");
        item.SpriteLayer = 21;
        if (item.Stack.item is Hoe || item.Stack.item is Axe || item.Stack.item is Hammer)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.gameObject.SetActive(true);
            heldItem.SpriteLayer = 10;
            heldItem.transform.SetParent(null);
            SceneManager.MoveGameObjectToScene(heldItem.gameObject, SceneManager.GetSceneByBuildIndex(StateManager.GAME_SCENE_INDEX));
            heldItem.transform.position = playerObject.GetComponent<PlayerInteractor>().interactionPoint.position;
            heldItem.gameObject.layer = LayerMask.NameToLayer("Interactable");
            heldItem = null;
        }
    }

    public void DestroyHeldItem()
    {
        var item = heldItem;
        DropItem();
        Object.Destroy(item.gameObject);
    }
}