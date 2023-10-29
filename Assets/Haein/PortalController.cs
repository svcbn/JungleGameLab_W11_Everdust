using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public GameObject textParent;
    private bool isPlayerIn = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (isPlayerIn)
            {
                //스테이지 변경
                Debug.Log("스테이지 변경됨");
                SceneManager.LoadScene("MinJoon");
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textParent.SetActive(true);
            isPlayerIn = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textParent.SetActive(false);
            isPlayerIn = false;
        }
    }
}
