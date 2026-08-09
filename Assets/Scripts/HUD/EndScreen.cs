using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text wheatGained;
    [SerializeField] private TMP_Text resourcesSold;
    [SerializeField] private TMP_Text questsCompleted;
    [SerializeField] private TMP_Text playersLost;

    void Start()
    {
        var s = Stats.Instance;
        title.text = s.Victory ? "Victoire" : "Defaite";
        wheatGained.text = s.WheatGained.ToString();
        resourcesSold.text = s.ResourcesSold.ToString();
        questsCompleted.text = s.QuestsCompleted.ToString();
        playersLost.text = s.PlayersLost.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(StateManager.PLAYER_SELECT_SCENE_INDEX);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(StateManager.MAIN_MENU_SCENE_INDEX);
    }
}
