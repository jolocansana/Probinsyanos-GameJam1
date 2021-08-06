using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMechanics : MonoBehaviour
{
    private List<string> inventory;
    private int timer;

    [SerializeField] private List<GameObject> goldbergComponents;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject TimerText;
    [SerializeField] private GameObject PromptCanvas;
    [SerializeField] private GameObject PlayerObject;
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

        // test
        this.StartCoroutine(this.UpdateTimer());
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
    }

    void triggerPrompt(Parameters param)
    {
        bool trigger = param.GetBoolExtra("trigger", false);
        string text = param.GetStringExtra("prompt_text", "");
        if (trigger)
        {
            PromptCanvas.GetComponentInChildren<Text>().text = text;
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
        foreach (string x in inventory)
        {
            GameObject component = goldbergComponents.Find(
                        delegate (GameObject gameObject)
                        {
                            return gameObject.name.Equals(x);
                        }
                    );
            if (component) component.SetActive(true);
        }
    }

    // When picking up an item
    void AddInventory(Parameters param)
    {
        string itemName = param.GetStringExtra("itemName", "none");
        inventory.Add(itemName);
        foreach(string x in inventory)
        {
            Debug.Log(x);
        }
    }

    void GameOver()
    {
        // gameover logic via winning or times up
        gameOverCanvas.SetActive(true);

        // disable player
        PlayerObject.SetActive(false);
        defaultCamera.SetActive(true);
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
        else
        {
            this.StartCoroutine(this.UpdateTimer());
        }
    }
}