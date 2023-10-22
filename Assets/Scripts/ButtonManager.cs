using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using AustinHarris.JsonRpc;


public class ButtonManager : MonoBehaviour
{

    [SerializeField] private TMP_Text CurrentBalance;
    [SerializeField] private TMP_Text Stage;
    [SerializeField] private TMP_Text Pot;
    private string agentInputAction;
    public string PlayerAction = "Nothing";
    public static ButtonManager Instance;

    public void Call() // Call button
    {
        if (GameManager.Instance.playerTurnEnd) //if player already played this round
        {
            GameManager.Instance.StatusMessage.text = "Not your turn";
            StartCoroutine(Delay(2F));
            return;
        }
        else
        {
            PlayerAction = "call";
            GameManager.Instance.playerBalance -= GameManager.Instance.formerPlayerBet;
            GameManager.Instance.playerTurnEnd = true;
        }
    }
    public void Fold() // fold button
    {
        if (GameManager.Instance.playerTurnEnd) //if player already played this round
        {
            GameManager.Instance.StatusMessage.text = "Not your turn";
            StartCoroutine(Delay(2F));
            return;
        }
        else
        {
            GameManager.Instance.playerTurnEnd = true;
            PlayerAction = "fold";
        }
    }

    public void Check()
    {
        if (GameManager.Instance.playerTurnEnd)
        { //if player already played this round
            GameManager.Instance.StatusMessage.text = "Not your turn";
            StartCoroutine(Delay(2F));
            return;
        }
        else
        {
            GameManager.Instance.playerTurnEnd = true;
            PlayerAction = "check";
        }
    }


    class Rpc : JsonRpcService
    {
        ButtonManager buttonManager;
        GameManager gameManager;
        public Rpc(ButtonManager buttonManager)
        {
            this.buttonManager = buttonManager;
        }

        [JsonRpcMethod]
        public string GetAction()
        {
            return buttonManager.PlayerAction;
        }
        // [JsonRpcMethod]
        // public void InputAgentAction(string action)
        // {
        //     buttonManager.agentInputAction = action;
        //     switch (action)
        //     {
        //         case "Call":
        //             buttonManager.Call();
        //             Debug.Log("Call");
        //             break;
        //         case "Fold":
        //             buttonManager.Fold();
        //             Debug.Log("Fold");
        //             break;
        //         case "Check":
        //             buttonManager.Check();
        //             Debug.Log("Check");
        //             break;
        //         case "Raise":
        //             Debug.Log("Raise");
        //             //gameManager.AgentRaise(2 * gameManager.Instance.formerPlayerBet);
        //             break;
        //         default:
        //             Debug.Log("Invalid action");
        //             break;
        //     }
        // }
        [JsonRpcMethod]
        public bool RestartGame(bool state)
        {
            return state;
        }
    }

    Rpc rpc;
    private void Start()
    {
        Instance = this;
        rpc = new Rpc(this);
    }

    public void Restart() // Restart button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        rpc.RestartGame(GameManager.Instance.wantToRestart = true);
        //GameManager.Instance..text = "";
    }
    IEnumerator Delay(float delayTime) // delay
    {
        yield return new WaitForSeconds(delayTime);
        GameManager.Instance.StatusMessage.text = "";
        // CurrentBalance.text = "Balance: $" + GameManager.Instance.playerBalance.ToString();
    }
}