using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // This index refers to the children of component using this Script, with 0 being the first child
    public int selectedWeapon = 0;
    [SerializeField] private ARPickUp ARPickUp;
    
    

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if(ARPickUp.arPickedUp)
        {
            SelectWeaponWithScroll();
            SelectWeaponWithKey();
        }      
    }

    void SelectWeaponWithScroll()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeaponWithKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
            SelectWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
    } 
}
