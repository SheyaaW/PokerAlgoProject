using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Threading.Tasks;
using AustinHarris.JsonRpc;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerBalance;
    private int userBet;
    public TMP_Text StatusMessage;
    [SerializeField] private TMP_Text potText;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private GameObject dealerButtonGameObject;
    [SerializeField] private TMP_Text BalanceAdditionText;
    [SerializeField] private TMP_Text PotAdditionText;
    [SerializeField] private TMP_Text BalanceText;
    public bool playerTurnEnd = false;
    public int potMoney;
    public int formerPlayerBet;
    [SerializeField] private GameObject WinScene;
    [SerializeField] private GameObject LoseScene;
    public bool wantToRestart = false;
    class Rpc : JsonRpcService
    {
        GameManager gameManager;
        ButtonManager buttonManager;
        SoundEffect soundEffect;
        public Rpc(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        [JsonRpcMethod]
        public void GetPot(string pot_amount)
        {
            //Debug.Log(pot_amount);

            gameManager.potMoney = int.Parse(pot_amount);
            gameManager.UpdatePotText(gameManager.potMoney);
        }

        [JsonRpcMethod]
        public void WhoWon(string winner)
        {
            Debug.Log($"Winner is {winner}");
            gameManager.StatusMessage.text = winner + " won!";
            if (winner == "Player")
            {
                gameManager.WinScene.SetActive(true);
                soundEffect.WinSound();
            }
            else
            {
                gameManager.LoseScene.SetActive(true);
                soundEffect.LoseSound();
            }
        }
        [JsonRpcMethod]
        public int SendPlayerBet(int bet)
        {
            return bet;
        }
    }

    Rpc rpc;

    private void Start()
    {
        playerBalance = 100;
        rpc = new Rpc(this);
        BalanceText.text = "Balance: $" + playerBalance.ToString();
    }

    private void Awake()
    {
        Instance = this;
    }
    public void UpdatePotText(int amountPot)
    {
        potText.text = "Pot: $" + amountPot.ToString();
        PotAdditionText.color = Color.green;
        PotAdditionText.text = "+$" + amountPot.ToString();
        StartCoroutine(Delay(2F));
    }


    public int GetBetAmount(int bet) // Get the bet amount from the input field
    {
        if (bet < 0)// If the bet is negative
        {
            StatusMessage.text = "Please enter a positive number";
            StartCoroutine(Delay(2F));
        }
        else if (bet < (formerPlayerBet * 2))
        {
            StatusMessage.text = "You have to bet at least twice the previous bet";
            StartCoroutine(Delay(2F));
        }
        else if (bet > playerBalance) // If the bet is greater than the balance
        {
            StatusMessage.text = "Not enough money";
            StartCoroutine(Delay(2F));
        }
        else if (bet == 0) // If the bet is 0
        {
            StatusMessage.text = "Can not bet $0";
            StartCoroutine(Delay(2F));
        }
        else // If the bet is valid
        {
            BalanceAdditionText.text = "-$" + bet.ToString();
            PotAdditionText.text = "+$" + bet.ToString();
            StartCoroutine(Delay(2F));
            userBet = bet;
            playerBalance -= bet;
            potMoney += bet;
            BalanceText.text = "Balance: $" + playerBalance.ToString();
            playerTurnEnd = true; //End turn
            rpc.SendPlayerBet(userBet);
            return playerBalance;
        }
        return playerBalance;
    }

    public int AgentRaise(int agentBet)
    {
        if (agentBet < (formerPlayerBet * 2))
        {
            StatusMessage.text = "Invalid Raise";
            StartCoroutine(Delay(2F));
        }
        return agentBet;
    }

    // private void Showdown()
    // {
    //     if (playerBalance > botBalance && playerBalance > agentBalance)
    //     {
    //         WinScene.SetActive(true);
    //     }
    //     else if (botBalance > playerBalance && botBalance > agentBalance)
    //     {
    //         LoseScene.SetActive(true);
    //     }
    // }

    IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StatusMessage.text = "";
        BalanceAdditionText.text = "";
        PotAdditionText.text = "";
    }



}