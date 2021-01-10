using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRandomSceneABTest : MonoBehaviour
{
    [Header("(Inc min, exc max)Random scene tussen twee waardes")]
    [SerializeField] int minScenesIndexInc = 1;
    [SerializeField] int maxScenesIndexExc = 3;

    void Start()
    {
        int num = Random.Range(minScenesIndexInc, maxScenesIndexExc);
        Debug.Log(num);

        SceneManager.LoadScene(num);
    }

}
