using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Mainmenu : MonoBehaviour
{



    public void IwasClicked()
    {
        SceneManager.LoadScene(0);
    }
}