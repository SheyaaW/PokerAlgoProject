using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ResetGame : MonoBehaviour
{
    bool ResetBtnPressed()
    {
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return true;
        }
        return false;
    }
    void Update()
    {
        ResetBtnPressed();
    }
}
