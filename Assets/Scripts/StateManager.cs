using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;
    [SerializeField] private GameObject garry;

    [SerializeField] private int _wheat = 0;
    public int Wheat
    {
        get => _wheat; set
        {
            if (value > _wheat)
            {
                Stats.Instance.WheatGained += value - _wheat;
            }

            _wheat = value;
        }
    }

    public int Tier { get; private set; }

    public bool isInPlayerSelect = false;
    public bool isInGame = false;
    public bool isInEndScreen = false;
    public bool tutorialEnabled = true;

    public bool hasPlayerDied = false;
    public bool hasPlayerDiedTutorialDone = false;
    public bool tutorialStarted = false;
    public bool hasTilled = false;
    public bool hasTilledTutorialDone = false;
    public bool hasGrabbedBanner = false;
    public bool hasGrabbedBannerTutorialDone = false;
    public bool hasPlanted = false;
    public bool hasPlantedTutorialDone = false;
    public bool hasDeposited = false;
    public bool hasDepositedTutorialDone = false;
    public bool hasPickedTicket = false;
    public bool hasPickedTicketTutorialDone = false;
    public bool hasUsedTicket = false;
    public bool hasUsedTicketTutorialDone = false;
    public bool hasCompletedQuest = false;
    public bool hasCompletedQuestTutorialDone = false;
    public bool hasBoughtSeeds = false;
    public bool hasBoughtSeedsTutorialDone = false;
    public bool hasRecipes = false;
    public bool hasT1QuestTutorialDone = false;
    public bool hasCrafted = false;
    public bool hasCraftedTutorialDone = false;

    public bool showDelta = false;
    public bool canGenerateQuest = true;
    public bool generateQuestNow = false;

    public const int MAIN_MENU_SCENE_INDEX = 0;
    public const int PLAYER_SELECT_SCENE_INDEX = 1;
    public const int GAME_SCENE_INDEX = 4;
    public const int END_SCREEN_SCENE_INDEX = 3;
    public const string TUTORIAL_SOURCE = "Garry";

    void SendTutorial(string dialog) { DialogManager.Instance.AddDialog(TUTORIAL_SOURCE, dialog, "blue"); }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Instance = this;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == PLAYER_SELECT_SCENE_INDEX)
        {
            isInPlayerSelect = true;
            isInGame = false;
        }
        else if (scene.buildIndex == GAME_SCENE_INDEX)
        {
            isInPlayerSelect = false;
            isInGame = true;
            isInEndScreen = false;
            canGenerateQuest = !tutorialEnabled;
            generateQuestNow = false;
            tutorialStarted = false;
            hasPlayerDied = false;
            hasPlayerDiedTutorialDone = false;
            hasTilled = false;
            hasTilledTutorialDone = false;
            hasPlanted = false;
            hasPlantedTutorialDone = false;
            hasDeposited = false;
            hasDepositedTutorialDone = false;
            hasPickedTicket = false;
            hasPickedTicketTutorialDone = false;
            hasUsedTicket = false;
            hasUsedTicketTutorialDone = false;
            hasCompletedQuest = false;
            hasCompletedQuestTutorialDone = false;
            hasBoughtSeeds = false;
            hasBoughtSeedsTutorialDone = false;
            hasRecipes = false;
            hasT1QuestTutorialDone = false;
            hasCrafted = false;
            hasCraftedTutorialDone = false;
        }
        else if (scene.buildIndex == END_SCREEN_SCENE_INDEX)
        {
            isInPlayerSelect = false;
            isInGame = false;
            isInEndScreen = true;
        }
    }

    void Update()
    {
        if (tutorialEnabled && isInGame)
        {
            if (hasPlayerDied && !hasPlayerDiedTutorialDone)
            {
                hasPlayerDiedTutorialDone = true;
                if (!hasGrabbedBannerTutorialDone)
                {
                    SendTutorial("...");
                    StartCoroutine(SendMessageWithDelay(TUTORIAL_SOURCE, "T'abuse là, carrément je les aide pas à te sauver", "blue", 2f));
                    StartCoroutine(ShowGarry(4f));
                }
                else if (!hasTilledTutorialDone)
                {
                    SendTutorial("Ah mince! Il fallait suivre ton ami!");
                    SendTutorial("Pour te sauver, il va devoir te racheter au magasin");
                    StartCoroutine(ShowGarry(3f));
                }
                else
                {
                    SendTutorial("Quelqu'un s'est perdu! Vous pouvez le retrouver au magasin et le racheter");
                    StartCoroutine(ShowGarry(3f));
                }
            }
            if (!tutorialStarted)
            {
                tutorialStarted = true;
                SendTutorial("Salut, je suis Garry et je vais vous apprendre les bases du business de la création friandise");
                SendTutorial("Pour commencer, il vous faut ramasser la bannière afin de ne pas vous perdre");
                StartCoroutine(ShowGarry(7f));
            }
            if (hasGrabbedBanner && !hasGrabbedBannerTutorialDone)
            {
                hasGrabbedBannerTutorialDone = true;
                SendTutorial("À présent ramassez cette houe et allez labourer des parcelles de terrain");
                StartCoroutine(ShowGarry(3f));
            }
            if (hasTilled && !hasTilledTutorialDone && hasGrabbedBannerTutorialDone)
            {
                hasTilledTutorialDone = true;
                SendTutorial("Maintenant, achetez des graines et semez les dans votre champ");
                StartCoroutine(ShowGarry(3f));
            }
            if (hasBoughtSeeds && !hasBoughtSeedsTutorialDone)
            {
                hasBoughtSeedsTutorialDone = true;
            }
            if (hasPlanted && !hasPlantedTutorialDone)
            {
                hasPlantedTutorialDone = true;
                SendTutorial("Les bonbons sont des plantes qui prennent leur temps pour pousser");
                SendTutorial("Je vais donc en profiter pour vous expliquer ce que vous faites ici");
                SendTutorial("Votre objectif est de livrer les commandes des clients, afin d'améliorer votre confiserie");
                SendTutorial("Lorsque les bonbons auront poussés, amenez-les à la ferme");
                StartCoroutine(ShowGarry(10f));
            }
            if (hasDeposited && !hasDepositedTutorialDone)
            {
                hasDepositedTutorialDone = true;
                canGenerateQuest = true;
                generateQuestNow = true;
                SendTutorial("Vous pouvez savoir en permanence ce qui se trouve dans votre ferme en regardant en haut à gauche");
                SendTutorial("Maintenant que vous avez récolté des bonbons, aller récupérer une commande au comptoir");
                StartCoroutine(ShowGarry(5f));

            }
            if (hasPickedTicket && !hasPickedTicketTutorialDone)
            {
                hasPickedTicketTutorialDone = true;
                SendTutorial("Maintenant que vous avez un ticket de commande, retournez à la ferme et prennez ce qu'il vous demande");
                StartCoroutine(ShowGarry(3f));
            }
            if (hasUsedTicket && !hasUsedTicketTutorialDone)
            {
                hasUsedTicketTutorialDone = true;
                SendTutorial("Désormais que vous avez la commande je vous laise deviner quoi faire");
                SendTutorial("Réponse A: le ramener au client");
                SendTutorial("Réponse B: allez dormir");
                StartCoroutine(ShowGarry(5f));
            }
            if (hasCompletedQuest && !hasCompletedQuestTutorialDone)
            {
                hasCompletedQuestTutorialDone = true;
                SendTutorial("Maintenant vous avez du blé. CAPITALISME!!! Tentez d'acheter des améliorations pour votre ferme afin de débloquer des graines au shop");
                SendTutorial("Moi j'ai fini je vais jouer à un meilleur jeu");
                StartCoroutine(ShowGarry(5f));
                StartCoroutine(SendMessageWithDelay("", "<color=red> Garry a été tué par un zombie </color>", "red", 7f));
                StartCoroutine(SendMessageWithDelay("Player 1", "wtf", "yellow", 9f));
            }
            if (hasRecipes && !hasT1QuestTutorialDone)
            {
                hasT1QuestTutorialDone = true;
                SendTutorial("Je l'ai fait exprès de dire que j'étais mort");
                SendTutorial("Mais je suis de retour pour vous apprendre les craft (concept 100% original)");
                SendTutorial("Vas donc à l'établi et sélectionne la recette. Ensuite, tu peux la concevoir directement à partir des ingrédients présents dans la ferme");
                StartCoroutine(ShowGarry(7f));
            }
            if (hasCrafted && !hasCraftedTutorialDone)
            {
                hasCraftedTutorialDone = true;
                SendTutorial("Bon cette fois je pars pour de bon");
                StartCoroutine(ShowGarry(2f));
                StartCoroutine(SendMessageWithDelay("", "<color=red> Garry a quitté la partie pour de bon</color>", "red", 2f));
            }
        }
    }

    public void UpgradeTier()
    {
        Tier += 1;

        if (Tier == 3)
        {
            EndGame(true);
        }

        ItemManager.Instance.UpgradeTier();
    }

    public void EndGame(bool victory)
    {
        foreach (Player player in PlayerManager.Instance.players.Values.ToList())
        {
            Destroy(player.playerObject);
        }
        Stats.Instance.Victory = victory;
        SceneManager.LoadScene(END_SCREEN_SCENE_INDEX);
    }

    IEnumerator SendMessageWithDelay(string source, string line, string color, float delay)
    {
        yield return new WaitForSeconds(delay);
        DialogManager.Instance.AddDialog(source, line, color);
    }

    IEnumerator ShowGarry(float time)
    {
        if (garry == null)
        {
            garry = GameObject.FindGameObjectWithTag("Garry");
        }
        garry.SetActive(true);
        yield return new WaitForSeconds(time);
        garry.SetActive(false);
    }
}
