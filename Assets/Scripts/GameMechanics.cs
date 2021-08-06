using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMechanics : MonoBehaviour
{
    private List<string> inventory;
    private int timer;

    private int arnis_count = 0;
    private int pipe_count = 0;
    private int domino_count = 0;

    private Transform arnis_transform;

    private bool game_end = false;

    [SerializeField] private List<GameObject> goldbergComponents;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject winnerCanvas;
    [SerializeField] private GameObject TimerText;
    [SerializeField] private GameObject InventoryText;
    [SerializeField] private GameObject PromptCanvas;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject UIComponent;
    [SerializeField] private GameObject defaultCamera;

    // Start is called before the first frame update
    void Start()
    {
        timer = 60; // in seconds

        inventory = new List<string>();
        EventBroadcaster.Instance.AddObserver(EventNames.GameJam.CHECK_INVENTORY, this.CheckInventory);
        EventBroadcaster.Instance.AddObserver(EventNames.GameJam.ADD_INVENTORY, this.AddInventory);
        EventBroadcaster.Instance.AddObserver(EventNames.GameJam.GAME_OVER, this.GameOver);
        EventBroadcaster.Instance.AddObserver(EventNames.GameJam.TRIGGER_PROMPT, this.triggerPrompt);
        EventBroadcaster.Instance.AddObserver(EventNames.GameJam.MACHINE_COMPLETE, this.machineComplete);

        // test
        this.StartCoroutine(this.UpdateTimer());

        InventoryText.GetComponent<Text>().text =
            "Inventory:\n" +
            "Arnis Stick: " + arnis_count + "/1\n" +
            "Pipe: " + pipe_count + "/1\n" +
            "Dominios: " + domino_count + "/3\n\n" +
            "Complete items before clicking the red button!\n";

        // Tutorial Prompt
        this.StartCoroutine(this.triggerTutorial());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.GameJam.CHECK_INVENTORY);
        EventBroadcaster.Instance.RemoveObserver(EventNames.GameJam.ADD_INVENTORY);
        EventBroadcaster.Instance.RemoveObserver(EventNames.GameJam.GAME_OVER);
        EventBroadcaster.Instance.RemoveObserver(EventNames.GameJam.TRIGGER_PROMPT);
        EventBroadcaster.Instance.RemoveObserver(EventNames.GameJam.MACHINE_COMPLETE);
    }

    void triggerPrompt(Parameters param)
    {
        bool trigger = param.GetBoolExtra("trigger", false);
        string text = param.GetStringExtra("prompt_text", "");
        if (trigger)
        {
            PromptCanvas.GetComponentInChildren<Text>().text = text;
            PromptCanvas.GetComponentInChildren<Text>().fontSize = 18;
            PromptCanvas.SetActive(true);
        } 
        else
        {
            PromptCanvas.SetActive(false);
        }
    }

    // Used to check when attaching a component to goldberg
    void CheckInventory(Parameters param)
    {
        string itemName = param.GetStringExtra("itemName", "none");

        Debug.Log("Checking Inventory");

        // loop through inventory array: if itemName exists, reveal in goldberg, else, prompt user that item is not in inventory.
        if (arnis_count == 1 && pipe_count == 1 && domino_count == 3)
        {
            foreach (string x in inventory)
            {
                Debug.Log(x);
                GameObject component = goldbergComponents.Find(
                            delegate (GameObject gameObject)
                            {
                                return gameObject.name.Equals(x);
                            }
                        );
                component.SetActive(true);
            }

            EventBroadcaster.Instance.PostEvent(EventNames.GameJam.RESET_GOLDBERG);

        }
        //else
        //{
        //    PromptCanvas.GetComponentInChildren<Text>().text = "Incomplete items!";
        //    PromptCanvas.GetComponentInChildren<Text>().fontSize = 18;
        //    PromptCanvas.SetActive(true);
        //}
    }


    // When picking up an item
    void AddInventory(Parameters param)
    {
        string itemName = param.GetStringExtra("itemName", "none");

        switch(itemName)
        {
            case "Arnis_Dirty":
                arnis_count++;
                inventory.Add(itemName);
                break;
            case "Pipe_Hide":
                pipe_count++;
                inventory.Add(itemName);
                break;
            default:
                domino_count++;
                if (domino_count == 3)
                {
                    inventory.Add("HiddenDomino");
                }
                break;
        }

        // foreach (string x in inventory)
        // {
        //    Debug.Log(x);
        //}


        InventoryText.GetComponent<Text>().text =
            "Inventory:\n" +
            "Arnis Stick: " + arnis_count + "/1\n" +
            "Pipe: " + pipe_count + "/1\n" +
            "Dominios: " + domino_count + "/3\n\n" +
            "Complete items before clicking the red button!\n";

    }

    void GameOver()
    {
        // gameover logic via winning or times up
        gameOverCanvas.SetActive(true);

        // disable player
        PlayerObject.SetActive(false);
        defaultCamera.SetActive(true);
        game_end = true;
        UIComponent.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    void machineComplete()
    {
        // gameover logic via winning or times up
        winnerCanvas.SetActive(true);

        // disable player
        PlayerObject.SetActive(false);
        defaultCamera.SetActive(true);
        game_end = true;
        UIComponent.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }


    IEnumerator UpdateTimer()
    {
        yield return new WaitForSeconds(1.0f);

        timer -= 1;
        this.TimerText.GetComponent<Text>().text =
            "TIME LEFT: " +
            timer / 60 + ":" + // get minutes
            timer % 60; // get seconds

        if (this.timer < 10)
        {
            this.TimerText.GetComponent<Text>().color = Color.red;
        }

        if (this.timer <= 0)
        {
            this.GameOver();
        }
        else if (!game_end)
        {
            this.StartCoroutine(this.UpdateTimer());
        }
    }

    IEnumerator triggerTutorial()
    {
        PromptCanvas.GetComponentInChildren<Text>().text = "WASD to move. Collect the missing items in your inventory! Press the red button to run the goldberg machine when complete.";
        PromptCanvas.GetComponentInChildren<Text>().fontSize = 12;
        PromptCanvas.SetActive(true);

        yield return new WaitForSeconds(10.0f);
        PromptCanvas.SetActive(false);
    }
}