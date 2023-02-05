using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public void FuckingRestart () {
        UnityEngine.SceneManagement.SceneManager.LoadScene ( 0 );
    }
}
