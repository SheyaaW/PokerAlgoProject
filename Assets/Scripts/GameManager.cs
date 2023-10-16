using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    private int cardsOnHand;
    public static GameManager Instance;
    public int playerBalance;
    public int botBalance;
    public int agentBalance;
    private int userBet;
    private int botBet;
    private int agentBet;
    //public GameState State;
    public TMP_Text StatusMessage;
    [SerializeField] private TMP_Text potText;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private GameObject dealerButtonGameObject;
    [SerializeField] private TMP_Text BalanceAdditionText;
    [SerializeField] private TMP_Text PotAdditionText;
    public bool playerTurnEnd = false;
    public int potMoney;
    public int formerPlayerBet;
    [SerializeField] private GameObject WinScene;
    [SerializeField] private GameObject LoseScene;
    //public static event Action<GameState> OnGameStateChanged;
    // private int currentPlayerIndex = 0;
    // public string currentPlayer;
    private string[] players = { "Player", "Bot", "Agent" };

    private void Awake()
    {
        Instance = this;
        playerBalance = 100; // Set the balance to 1000
        botBalance = 100; // Set the balance to 1000
        agentBalance = 100; // Set the balance to 1000
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
            playerTurnEnd = true; //End turn
            return playerBalance;
        }
        return playerBalance;
    }

    public int AgentRaise(int agentBet){
        if (agentBet < (formerPlayerBet * 2))
        {
            StatusMessage.text = "Invalid Raise";
            StartCoroutine(Delay(2F));
        }
        return agentBet;
    }

    private void Showdown()
    {
        if (playerBalance > botBalance && playerBalance > agentBalance)
        {
            WinScene.SetActive(true);
        }
        else if (botBalance > playerBalance && botBalance > agentBalance)
        {
            LoseScene.SetActive(true);
        }
    }
    IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StatusMessage.text = "";
        BalanceAdditionText.text = "";
        PotAdditionText.text = "";
    }
}