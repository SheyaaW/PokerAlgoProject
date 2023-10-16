// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class UIManager : MonoBehaviour
// {
//     [SerializeField] private GameObject StartGamePanel, Call, Raise, Fold, Check;
//     [SerializeField] private TextMeshProUGUI Phase;
//     private void Awake() {
//         GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
//     }
//     private void OnDestroy() {
//         GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
//     }
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     private void GameManager_OnGameStateChanged(GameState state)
//     {
//         StartGamePanel.SetActive(state == GameState.Menu);

        
//     }

//     public void CallButton()
//     {
        
//         Debug.Log("Call");
//     }
//     // Update is called once per frame
//     void Update()
//     {

//     }
// }
