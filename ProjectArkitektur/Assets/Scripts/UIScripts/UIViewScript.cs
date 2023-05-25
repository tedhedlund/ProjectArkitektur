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
    [SerializeField] TMP_Text ammoMagText;
    [SerializeField] TMP_Text ammoTotalText;
    [SerializeField] TMP_Text killsText;

    public void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
    }

    public void UpdateAmmoMag(int ammo)
    {
        ammoMagText.text = ammo.ToString();
    }

    public void UpdateAmmoTotal(int ammo)
    {
        ammoTotalText.text = ammo.ToString();
    }

    public void UpdateKills(int kills)
    {
        killsText.text = kills.ToString();
    }

}
