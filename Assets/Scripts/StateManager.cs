using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    public uint Tier;
    public bool isInPlayerSelect = false;
    public bool isInGame = false;
    public bool tutorialEnabled = true;

    public bool tutorialStarted = false;
    public bool hasTilled = false;
    public bool hasTilledTutorialDone = false;
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

    public const int PLAYER_SELECT_SCENE_INDEX = 1;
    public const int GAME_SCENE_INDEX = 2;
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
                SendTutorial("Salut, je suis Garry et je vais vous apprendre les base du businesse de friandise");
                //SendTutorial("Pour commancer, ramasse cet houe et vas houer des parcelle de terrin");
                SendTutorial("Pour commencer, il vous faut ramacer la bagnière afin de bouger votre point de repère pour ne pas vous perdre");

            }
            if (hasGrabbedBanner && !hasGrabbedBannerTutorialDone)
            {
                SendTutorial("À présent ramassez cet houe et allez houer des parcelle de terrin");
            }
            if (hasTilled && !hasTilledTutorialDone)
            {
                hasTilledTutorialDone = true;
                SendTutorial("Maintenant prennez des graine et allez les semer");
            }
            if (hasPlanted && !hasPlantedTutorialDone)
            {
                hasPlantedTutorialDone = true;
                SendTutorial("Les bonbons sont des plantes qui prenne leur temps pour pousser");
                SendTutorial("Je vais donc en profiter pour vous expliquer ce que vous faite ici");
                SendTutorial("Votre objectif est de délivrer les commandes des clients, afin d'ameliorer votre bonbonerie");
                SendTutorial("Lorce que les bonbons aurons pousser, amenez-les à la base");
            }
            if (hasDeposited && !hasDepositedTutorialDone)
            {
                hasDepositedTutorialDone = true;
                SendTutorial("Vous pouvez savoir en permanance ce qui se trouve dans votre base en regaradant en haut à gauche");
                SendTutorial("Maintenant que vous avez récolter des bonbons, aller récupérer une commande au contoire");

            }
            if (hasPickedTicket && !hasPickedTicketTutorialDone)
            {
                hasPickedTicketTutorialDone = true;
                SendTutorial("Maintenant que vous avez un tiquet de commande, retournez à la base et prener ce qu'il vous demande");
            }
            if (hasUsedTicket && !hasUsedTicketTutorialDone)
            {
                hasUsedTicketTutorialDone = true;
                SendTutorial("Maintenant que vous avez la commande je vous laise deviner quoi faire");
                SendTutorial("Raiponse A: le ramener au client");
                SendTutorial("Raiponse B: allez dormir");
            }
            if (hasCompletedQuest && !hasCompletedQuestTutorialDone)
            {
                hasCompletedQuestTutorialDone = true;
                SendTutorial("Maintenant vous avez de l'argent");
                SendTutorial("Allez donc dépenser cet argent durement gagner au magasin (CAPITALISME!!!)");
            }
            if (hasBoughtSeeds && !hasBoughtSeedsTutorialDone)
            {
                hasBoughtSeedsTutorialDone = true;
                SendTutorial("Maintenant que c'est fini je vais jouer à un meilleur jeu");
                SendTutorial("/ a quiter la partie /");
            }
            if (hasT1Quest && !hasT1QuestTutorialDone)
            {
                hasT1QuestTutorialDone = true;
                SendTutorial("J'ai fait exprès de faire croire que j'était mort");
                SendTutorial("Mais je suis de retour pour vous apprendre les craft (consepte 100% original)");
                SendTutorial("Vas donc à la table de craft et utilise la");
            }
            if (hasCrafted && !hasCraftedTutorialDone)
            {
                hasCraftedTutorialDone = true;
                SendTutorial("Bon cet fois je part pour de bon");
                SendTutorial("/ a quiter la partie pour de bon /");
            }
        }
    }
}
