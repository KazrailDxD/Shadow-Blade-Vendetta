using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PublisherLogo : MonoBehaviour
{

   [SerializeField] public GameObject publisherLogo = null;
   [SerializeField] public GameObject teamLogo = null;
   
    // Start is called before the first frame update
    void Start()
    {
        teamLogo.SetActive(false);
        publisherLogo.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(LogoSwitch());

        if (Input.anyKeyDown) 
        {
            LoadeMainMenue();
        }
    }

    private IEnumerator LogoSwitch() 
    {
        
        yield return new WaitForSeconds(3);
        publisherLogo.SetActive(false) ;
        teamLogo.SetActive(true);
        yield return new WaitForSeconds(3);
        LoadeMainMenue();
    }

    private void LoadeMainMenue() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
