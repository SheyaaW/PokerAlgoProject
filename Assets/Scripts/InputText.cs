using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputText : MonoBehaviour
{
    private int inputBet;
    public void ReadStringInput(string money) // Read the input from the input field
    {
        if (GameManager.Instance.playerTurnEnd)
        {
            GameManager.Instance.StatusMessage.text = "Not your turn";
            StartCoroutine(Delay(1.25F));
            return; // Do nothing if input has already been used
        }
        inputBet = int.Parse(money); // string to int
        ButtonManager.Instance.PlayerAction = "raise";
        GameManager.Instance.GetBetAmount(inputBet); //Get the bet amount from the input field
    }
    IEnumerator Delay(float time) // Delay the StatusMessage text
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.StatusMessage.text = ""; // Set the StatusMessage text to an empty string
    }
}


