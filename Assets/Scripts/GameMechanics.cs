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

    // Start is called before the first frame update
    void Start()
    {
        timer = 15; // in seconds

        inventory = new List<string>();
        EventBroadcaster.Instance.AddObserver(EventNames.GameJam.CHECK_INVENTORY, this.CheckInventory);
        EventBroadcaster.Instance.AddObserver(EventNames.GameJam.ADD_INVENTORY, this.AddInventory);
        EventBroadcaster.Instance.AddObserver(EventNames.GameJam.GAME_OVER, this.GameOver);

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
    }

    // Used to check when attaching a component to goldberg
    void CheckInventory(Parameters param)
    {
        string itemName = param.GetStringExtra("itemName", "none");

        // loop through inventory array: if itemName exists, reveal in goldberg, else, prompt user that item is not in inventory.
    }

    // When picking up an item
    void AddInventory(Parameters param)
    {
        string itemName = param.GetStringExtra("itemName", "none");
        inventory.Add(itemName);
    }

    void GameOver()
    {
        // gameover logic via winning or times up
        gameOverCanvas.SetActive(true);
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