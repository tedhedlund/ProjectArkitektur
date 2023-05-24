using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerScript : MonoBehaviour
{
    //Description
    //The controller class acts as an intermediary between the model and view. For example it could listen for button clicks to call view and model.
    [SerializeField] UIModelScript uiModel;    
    [SerializeField] Player_Controller pController;

    // Update is called once per frame
    void Update()
    {
        uiModel.playerHealth = pController.health;
        uiModel.UpdateViewlHealth();

    }
}
