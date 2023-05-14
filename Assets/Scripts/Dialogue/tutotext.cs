using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class tutotext : MonoBehaviour
{
    public GameObject UIObject;

    // Start is called before the first frame update
    void Start()
    {
        UIObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            UIObject.SetActive(true);
        Debug.Log("enterred");
        }
    }
    void Update()
    {

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIObject.SetActive(false);
        }
    }
   
}
