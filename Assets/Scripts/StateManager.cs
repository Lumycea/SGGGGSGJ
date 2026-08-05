using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public bool isInPlayerSelect = false;
    public bool isInGame = false;

    public const int PLAYER_SELECT_SCENE_INDEX = 1;
    public const int GAME_SCENE_INDEX = 2;

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == PLAYER_SELECT_SCENE_INDEX)
        {
            isInPlayerSelect = true;
            isInGame = false;
        }
        else if(SceneManager.GetActiveScene().buildIndex == GAME_SCENE_INDEX)
        {
            isInPlayerSelect = false;
            isInGame = true;
        }
    }
}
