using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AustinHarris.JsonRpc;


public class CardSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPointsBoard;
    [SerializeField] private Transform[] spawnPointsPlayerHand;
    [SerializeField] private Transform[] spawnPointsBotHand;
    [SerializeField] private Transform[] spawnPointsAgentHand;
    [SerializeField] private GameObject[] cardPrefabs;
    string cardIndex;
    class Rpc : JsonRpcService
    {
        CardSpawner cardSpawner;
        public Rpc(CardSpawner cardSpawner)
        {
            this.cardSpawner = cardSpawner;
        }

        [JsonRpcMethod]
        public void GetCardIndex(string index)
        {
            //eg. Input = 00P
            // 00 = card index
            // A = agent
            // B = board
            // C = bot
            // P = player
            Debug.Log($"Index: {index}");
            int cardIndex = int.Parse(index.Substring(0, 2));
            string who = index.Substring(2);

            // if (cardIndex == 0)
            // {
            //     Debug.LogError("No card to spawn!!!");
            //     return;
            // } // No card to spawn

            Debug.Log($"Card Index Test: {cardIndex}");
            Debug.Log($"Who: {who}");
            cardSpawner.SpawnCards(cardIndex + 1, who); // Spawn

        }
        [JsonRpcMethod]
        public void GetMessage(string message)
        {
            Debug.Log("Message: " + message);
        }
        [JsonRpcMethod]
        public void RevealCard(GameObject cardToReveal)// Pass in the card GameObject to reveal
        {
            SoundEffect.Instance.FlipCardSound();
            cardToReveal.transform.Rotate(0, 0, 0);
        }

    }

    Rpc rpc;
    private void Start()
    {
        cardPrefabs = Resources.LoadAll<GameObject>("Cards"); // Load all cards from Resources/Cards folder
        rpc = new Rpc(this);
    }

    int i;
    int j;
    int k;
    int l;
    private void SpawnCards(int CardsToSpawn, string Type)
    {
        if (Type == "P") // Spawn cards on Player's Hand
        {
            StartCoroutine(Delay(0.5f));
            GameObject card = Instantiate(cardPrefabs[CardsToSpawn], spawnPointsPlayerHand[i].position, Quaternion.identity); //Spawn cards on Player's Board
            i++;
        }
        else if (Type == "C") // Spawn cards on Bot's Hand
        {
            StartCoroutine(Delay(0.5f));
            GameObject card = Instantiate(cardPrefabs[CardsToSpawn], spawnPointsBotHand[j].position, Quaternion.identity);
            card.transform.Rotate(-180, 90, 0);
            j++;
        }
        else if (Type == "A") // Spawn cards on Agent's Hand
        {
            StartCoroutine(Delay(0.5f));
            GameObject card = Instantiate(cardPrefabs[CardsToSpawn], spawnPointsAgentHand[k].position, Quaternion.identity);
            card.transform.Rotate(-180, 90, 0);
            k++;
        }
        else if (Type == "B") // Spawn cards on Board
        {
            StartCoroutine(Delay(0.5f));
            GameObject card = Instantiate(cardPrefabs[CardsToSpawn], spawnPointsBoard[l].position, Quaternion.identity);
            card.transform.Rotate(0, 0, -180);
            l++;

        }
        else
        {
            Debug.LogError("Invalid Input from python!!!");
        }
    }


    IEnumerator Delay(float time) // Delay the StatusMessage text
    {
        SoundEffect.Instance.DrawCardSound(); // Set the StatusMessage text to an empty string
        yield return new WaitForSeconds(time);
    }
}