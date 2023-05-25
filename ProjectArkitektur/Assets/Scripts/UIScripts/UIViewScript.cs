using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIViewScript : MonoBehaviour
{
    //Description
    //View is responsible for displaying the UI elements. For example update the text with the current score.
    [SerializeField] UIModelScript uiModel;

    [SerializeField] TMP_Text healthText;

    public void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
    }

}
