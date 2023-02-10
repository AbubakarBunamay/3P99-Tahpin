using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_Manager : MonoBehaviour
{
    

    public void ChangeScene(int sceneBuildID)
    {
        SceneManager.LoadScene(sceneBuildID);
    }
}
