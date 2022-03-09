using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   
    private void OnEnable()
    {
        Boss.OnBossDied += SceneSwitch;
    }

    private void OnDisable()
    {
        Boss.OnBossDied -= SceneSwitch;
    }

   private void SceneSwitch() 
    {
        StartCoroutine(EndbossDead());
    }

    private IEnumerator EndbossDead() 
    {
        yield return new WaitForSeconds(7);       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
