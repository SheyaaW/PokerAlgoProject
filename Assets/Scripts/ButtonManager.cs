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
    private string lastAction = "Nothing";

    void Update() // Update text
    {
        CurrentBalance.text = "Balance: $" + GameManager.Instance.playerBalance.ToString();
        Pot.text = "Pot: $" + GameManager.Instance.potMoney.ToString();
    }

    public void Call() // Call button
    {
        if (GameManager.Instance.playerTurnEnd) //if player already played this round
        {
            GameManager.Instance.StatusMessage.text = "Not your turn";
            StartCoroutine(Delay(2F));
            return;
        }
        GameManager.Instance.playerTurnEnd = true;
        lastAction = "Call";
    }
    public void Fold() // fold button
    {
        if (GameManager.Instance.playerTurnEnd) //if player already played this round
        {
            GameManager.Instance.StatusMessage.text = "Not your turn";
            StartCoroutine(Delay(2F));
            return;
        }
        GameManager.Instance.playerTurnEnd = true;
        lastAction = "Fold";
    }

    public void Check()
    {
        if (GameManager.Instance.playerTurnEnd)
        { //if player already played this round
            GameManager.Instance.StatusMessage.text = "Not your turn";
            StartCoroutine(Delay(2F));
            return;
        }
        GameManager.Instance.playerTurnEnd = true;
        lastAction = "Check";
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
            return buttonManager.lastAction;
        }
        [JsonRpcMethod]
        public void InputAgentAction(string action)
        {
            buttonManager.agentInputAction = action;
            switch (action)
            {
                case "Call":
                    buttonManager.Call();
                    Debug.Log("Call");
                    break;
                case "Fold":
                    buttonManager.Fold();
                    Debug.Log("Fold");
                    break;
                case "Check":
                    buttonManager.Check();
                    Debug.Log("Check");
                    break;
                case "Raise":
                    Debug.Log("Raise");
                    gameManager.AgentRaise(2 * GameManager.Instance.formerPlayerBet);
                    break;
                default:
                    Debug.Log("Invalid action");
                    break;
            }
        }
    }

    Rpc rpc;
    private void Start()
    {
        rpc = new Rpc(this);
    }

    public void Restart() // Restart button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator Delay(float delayTime) // delay
    {
        yield return new WaitForSeconds(delayTime);
        GameManager.Instance.StatusMessage.text = "";
    }
}