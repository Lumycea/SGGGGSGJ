using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

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
    public bool tutorialEnabled = false;

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
    public bool hasT1Quest = false;
    public bool hasT1QuestTutorialDone = false;
    public bool hasCrafted = false;
    public bool hasCraftedTutorialDone = false;

    public bool showDelta = false;
    public bool canGenerateQuest = true;
    public bool generateQuestNow = false;

    public const int PLAYER_SELECT_SCENE_INDEX = 1;
    public const int GAME_SCENE_INDEX = 4;
    public const int END_SCREEN_SCENE_INDEX = 3;
    public const string TUTORIAL_SOURCE = "Garry";

    void SendTutorial(string dialog) { DialogManager.Instance.AddDialog(TUTORIAL_SOURCE, dialog); }

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
            canGenerateQuest = !tutorialEnabled;
            generateQuestNow = false;
            tutorialStarted = false;
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
            hasT1Quest = false;
            hasT1QuestTutorialDone = false;
            hasCrafted = false;
            hasCraftedTutorialDone = false;
        }
    }

    void Update()
    {
        if (tutorialEnabled && isInGame)
        {
            if (!tutorialStarted)
            {
                tutorialStarted = true;
                SendTutorial("Salut, je suis Garry et je vais vous apprendre les bases du business de la création friandise");
                SendTutorial("Pour commencer, il vous faut ramasser la bannière afin de bouger votre point de repère et de ne pas vous perdre");

            }
            if (hasGrabbedBanner && !hasGrabbedBannerTutorialDone)
            {
                SendTutorial("À présent ramassez cette houe et allez labourer des parcelles de terrain");
            }
            if (hasTilled && !hasTilledTutorialDone)
            {
                hasTilledTutorialDone = true;
                SendTutorial("Maintenant, prennez des graines et semez les dans votre champ");
            }
            if (hasPlanted && !hasPlantedTutorialDone)
            {
                hasPlantedTutorialDone = true;
                SendTutorial("Les bonbons sont des plantes qui prennent leur temps pour pousser");
                SendTutorial("Je vais donc en profiter pour vous expliquer ce que vous faites ici");
                SendTutorial("Votre objectif est de livrer les commandes des clients, afin d'améliorer votre confiserie");
                SendTutorial("Lorsque les bonbons auront poussés, amenez-les à la ferme");
            }
            if (hasDeposited && !hasDepositedTutorialDone)
            {
                hasDepositedTutorialDone = true;
                canGenerateQuest = true;
                generateQuestNow = true;
                SendTutorial("Vous pouvez savoir en permanence ce qui se trouve dans votre ferme en regardant en haut à gauche");
                SendTutorial("Maintenant que vous avez récolté des bonbons, aller récupérer une commande au comptoir");

            }
            if (hasPickedTicket && !hasPickedTicketTutorialDone)
            {
                hasPickedTicketTutorialDone = true;
                SendTutorial("Maintenant que vous avez un ticket de commande, retournez à la ferme et prennez ce qu'il vous demande");
            }
            if (hasUsedTicket && !hasUsedTicketTutorialDone)
            {
                hasUsedTicketTutorialDone = true;
                SendTutorial("Désormais que vous avez la commande je vous laise deviner quoi faire");
                SendTutorial("Réponse A: le ramener au client");
                SendTutorial("Réponse B: allez dormir");
            }
            if (hasCompletedQuest && !hasCompletedQuestTutorialDone)
            {
                hasCompletedQuestTutorialDone = true;
                SendTutorial("Maintenant vous avez du blé");
                SendTutorial("Allez donc dépenser cet argent durement gagner au magasin (CAPITALISME!!!)");
            }
            if (hasBoughtSeeds && !hasBoughtSeedsTutorialDone)
            {
                hasBoughtSeedsTutorialDone = true;
                SendTutorial("Maintenant que c'est fini je vais jouer à un meilleur jeu");
                SendTutorial("/ a quitté la partie /");
            }
            if (hasT1Quest && !hasT1QuestTutorialDone)
            {
                hasT1QuestTutorialDone = true;
                SendTutorial("Je l'ai fait exprès de dire que j'étais mort");
                SendTutorial("Mais je suis de retour pour vous apprendre les craft (concept 100% original)");
                SendTutorial("Vas donc à l'établi et sélectionne la recette. Ensuite, tu peux la concevoir directement à partir des ingrédients présents dans la ferme");
            }
            if (hasCrafted && !hasCraftedTutorialDone)
            {
                hasCraftedTutorialDone = true;
                SendTutorial("Bon cette fois je pars pour de bon");
                SendTutorial("/ a quitté la partie pour de bon /");
            }
        }
    }

    public void UpgradeTier()
    {
        Tier += 1;

        if (Tier == 3)
        {
            Stats.Instance.Victory = true;
            SceneManager.LoadScene(END_SCREEN_SCENE_INDEX);
        }

        ItemManager.Instance.UpgradeTier();
    }
}
