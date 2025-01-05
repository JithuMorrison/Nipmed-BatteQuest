using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour {
	public GameObject mainButton;
    public GameObject optionButton1;
    public GameObject optionButton2;
    public GameObject optionButton3;

    private bool isHandTouching = false;

    void Start()
    {
        optionButton1.SetActive(false);
        optionButton2.SetActive(false);
        optionButton3.SetActive(false);
    }

    void Update()
    {
        if (isHandTouching)
        {
            optionButton1.SetActive(true);
            optionButton2.SetActive(true);
            optionButton3.SetActive(true);
        }
        else{
            optionButton1.SetActive(false);
            optionButton2.SetActive(false);
            optionButton3.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hand"))
        {
            isHandTouching = !isHandTouching;
        }
    }
}
