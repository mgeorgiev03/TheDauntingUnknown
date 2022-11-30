using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSystem : MonoBehaviour
{
    public GameObject PopUpBox;
    public Animator animator;
    public Text popUpText;
    public Text popUpTitle;
    public void PopUp(string text , string titletext)
    {
        PopUpBox.SetActive(true);
        popUpTitle.text = titletext;
        popUpText.text = text;
        animator.SetTrigger("pop");
    }

    private void Update()
    {
        
    }
}
