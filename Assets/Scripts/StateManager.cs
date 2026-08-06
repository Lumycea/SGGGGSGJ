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
                SendTutorial("Hi! I'm Garry. I will teach you the basics of SGGGSGJ :)");
                SendTutorial("First, go pick the Hoe and use it to till some soil.");
            }
            if (hasTilled && !hasTilledTutorialDone)
            {
                hasTilledTutorialDone = true;
                SendTutorial("Great! Now, pick one of these seeds and plant them in your field.");
            }
            if (hasPlanted && !hasPlantedTutorialDone)
            {
                hasPlantedTutorialDone = true;
                SendTutorial("Plants all take some time to grow. In the meantime let me explain you some things.");
                SendTutorial("Your work here is to satisfy the people from Candy Land with sweet treats.");
                SendTutorial("Once a plant is grown, you can harvest it and bring it to the farm's silo.");
                SendTutorial("For now you only have a few crops, but you will soon need more to fill their requests.");
            }
            if (hasDeposited && !hasDepositedTutorialDone)
            {
                hasDepositedTutorialDone = true;
            }
            if (hasPickedTicket && !hasPickedTicketTutorialDone)
            {
                hasPickedTicketTutorialDone = true;
            }
            if (hasUsedTicket && !hasUsedTicketTutorialDone)
            {
                hasUsedTicketTutorialDone = true;
            }
            if (hasCompletedQuest && !hasCompletedQuestTutorialDone)
            {
                hasCompletedQuestTutorialDone = true;
            }
            if (hasBoughtSeeds && !hasBoughtSeedsTutorialDone)
            {
                hasBoughtSeedsTutorialDone = true;
            }
            if (hasT1Quest && !hasT1QuestTutorialDone)
            {
                hasT1QuestTutorialDone = true;
            }
            if (hasCrafted && !hasCraftedTutorialDone)
            {
                hasCraftedTutorialDone = true;
            }
        }
    }
}
