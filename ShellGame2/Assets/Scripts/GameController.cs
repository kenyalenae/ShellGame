using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreLogic;  // this is our plug in, or game logic
using System;
using HoloToolkit.Unity.InputModule;
using System.Text;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private int numberOfStrikes = 3;

    [SerializeField]
    private GameObject[] itemContainers;

    private ShellGameLogic coreLogic;

    private static GameController instance;  // to help make sure we only have one 

    private int score = 0;  // When the game starts up, this will cause the game to have a starting value of Zero

    private TextMesh strikeText;
    private TextMesh scoreText;
    private TextMesh highScoreText;

    //                                                               \/ Biggest the visiable striketext for user viewing will ever be ( X X X)  
    private StringBuilder strikeTextStringBuilder = new StringBuilder(6);  // used for keeping track of strikes for showing

    // to help make sure we only have one 
    public static GameController Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                GameObject go = GameObject.FindGameObjectWithTag(Res.GameController);
                instance = go.GetComponent<GameController>();

                return instance;
            }
        }
    }

    private void Awake()
    {
        coreLogic = new ShellGameLogic(itemContainers.Length, numberOfStrikes);
    }


    // Use this for initialization
    void Start()
    {
        FindTextObjects();

        HookupCoreLogicEvents();

        HighScore.LoadHighScore();

        UpdateHighScore();

        StartTurn();

    }

    private void FindTextObjects()
    {
        strikeText = GameObject.FindGameObjectWithTag(Res.StrikeText).GetComponent<TextMesh>();
        scoreText = GameObject.FindGameObjectWithTag(Res.ScoreText).GetComponent<TextMesh>();
        highScoreText = GameObject.FindGameObjectWithTag(Res.HighScoreText).GetComponent<TextMesh>();
    }

    private void StartTurn()
    {
        // Prepare items for the new turn and loop through the containers and make sure the peas are covered up
        // Hide the pea from the contianer so user can't see it, or cheat
        PrepareItemsForTurn();
        
        HideStrikeText();

        // Animate items 

        // Play animation sounds

        // Reset items
        coreLogic.ResetItems();

    }

    private void PrepareItemsForTurn()
    {
        for (int i = 0; i < itemContainers.Length; i++)  // Looping foreach box items 
        {
            itemContainers[i].GetComponent<MeshRenderer>().enabled = true;  // Make sure Mesh on boxes are enabled, to prevent player from seeing the pea
            itemContainers[i].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;  // This makes sure all the peas are hidden
        }
    }

    // Connects us to the main game logic and processes 
    private void HookupCoreLogicEvents()
    {
        coreLogic.StartTurn += CoreLogic_StartTurn;

        coreLogic.SelectedItem += CoreLogic_SelectedItem;
        coreLogic.CheckingItem += CoreLogic_CheckingItem;
        coreLogic.MatchNotMade += CoreLogic_MatchNotMade;
        coreLogic.MatchMade += CoreLogic_MatchMade;

        coreLogic.ItemReset += CoreLogic_ItemReset;
        coreLogic.ResetComplete += CoreLogic_ResetComplete;

        coreLogic.GameOver += CoreLogic_GameOver;
    }

    private void CoreLogic_StartTurn(object sender, EventArgs e)
    {
        Debug.Log("Start Turn");
        Invoke(nameof(StartTurn), 1.2f); // delay to allow for animations
    }

    // after disabling the items from user guess, this method will restore input from user
    private void CoreLogic_ItemReset(object sender, EventArgs e)
    {
        Debug.Log("Item Reset");

        // Restore input
        if (!InputManager.Instance.IsInputEnabled)
        {
            InputManager.Instance.PopInputDisable();
        }
    }

    private void CoreLogic_GameOver(object sender, EventArgs e)
    {
        // Used for displaying the score and current strikes when game over 
        Debug.Log($"GAME OVER. Score: {score} Strikes: {coreLogic.Strikes}");

        HighScore.Value = score;  // Will update the Highscore permanently if better then previous highscore 
        HighScore.SaveHighScore();

        score = 0;  // Resetting score to zero, for next game
        UpdateScore();  // this will display 0 for the player 

        strikeTextStringBuilder.Clear();  // getting rid of all the strikes in striketext
    }

    private void CoreLogic_ResetComplete(object sender, EventArgs e)
    {
        Debug.Log("Reset Complete");
    }

    private void CoreLogic_SelectedItem(object sender, ItemEventArgs e)  // User clicking on item will activate this if the pea is inside
    {
        Debug.Log($"Selected Item: {e.Id}");

        // Disable input, to prevent the user from selecting any of the other items 
        InputManager.Instance.PushInputDisable();

        // Show the pea / insert item
        itemContainers[e.Id].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;  // Will show the pea to user
    }

    private void CoreLogic_CheckingItem(object sender, ItemEventArgs e)  // User clicking on item will activate this if the pea is not inside
    {
        Debug.Log($"Checking Item: {e.Id}");
        itemContainers[e.Id].GetComponent<MeshRenderer>().enabled = false;  // This removes the Mesh from the box making insides visiable for user 
    }

    private void CoreLogic_MatchNotMade(object sender, NoMatchEventArgs e)
    {
        Debug.Log($"No Match Made. IsStrike: {e.IsStrike}");

        if (e.IsStrike)  // Prevent player from seeing strike if the just get first on wrong, but second guess right 
        {
            ShowStrikeText();  // This will show strike X to user 
        }
    }

    private void CoreLogic_MatchMade(object sender, MatchEventArgs e)
    {
        score += e.Score;  // Adds to your points, Yeah You!

        HighScore.Value = score;  // Will update the Highscore if better then previous highscore 
        UpdateHighScore();

        UpdateScore();  // Calls the method to update your score when doing well 
        Debug.Log($"Match Made. Id: {e.Id} Score: {e.Score} Total Score: {score}");
    }

    private void UpdateScore()
    {
        // This is for the display text to user and will update the score to show how they are doing 
        scoreText.text = $"{Res.Score}{score}";
    }

    private void UpdateHighScore()
    {
        // This is for the display text to user and will update the highscore if better then shown highscore
        highScoreText.text = $"{Res.HighScore}{HighScore.Value}";
    }

    private void ShowStrikeText() 
    {
        strikeTextStringBuilder.Append(Res.StrikeX); // adding the new strike to strikeText

        strikeText.text = strikeTextStringBuilder.ToString();  // Setting the strikeText's text 

        strikeText.gameObject.SetActive(true);  // allowing user to finially see it 
    }

    // This will hide strike text ( X X X) until another method activates it for the user to see. 
    private void HideStrikeText()
    {
        strikeText.gameObject.SetActive(false);
    }

    // used for when the user clicks in game 
    public void CheckForItem(int itemId)  
    {
        Debug.Log($"Check for pea: {itemId}");
        coreLogic.CheckForItem(itemId);  // Will raise MatchMade or MatchNotMade in CoreLogic\ShellGameLogic.cs
    }

    // If you do events, make sure you also distroy them when done with them
    private void OnDestroy()
    {
        coreLogic.StartTurn -= CoreLogic_StartTurn;

        coreLogic.SelectedItem -= CoreLogic_SelectedItem;
        coreLogic.CheckingItem -= CoreLogic_CheckingItem;
        coreLogic.MatchNotMade -= CoreLogic_MatchNotMade;
        coreLogic.MatchMade -= CoreLogic_MatchMade;

        coreLogic.ItemReset -= CoreLogic_ItemReset;
        coreLogic.ResetComplete -= CoreLogic_ResetComplete;

        coreLogic.GameOver -= CoreLogic_GameOver;
    }
}
