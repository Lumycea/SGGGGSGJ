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
        title.text = s.Victory ? "Victoire" : "Défaite";
        wheatGained.text = s.WheatGained.ToString();
        resourcesSold.text = s.ResourcesSold.ToString();
        questsCompleted.text = s.QuestsCompleted.ToString();
        playersLost.text = s.PlayersLost.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(StateManager.GAME_SCENE_INDEX);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
