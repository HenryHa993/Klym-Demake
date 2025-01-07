using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothing : MonoBehaviour
{
    public GameObject Hat;
    public GameObject Shirt;
    public GameObject Bottom;

    public void Wear(GameObject clothing)
    {
        clothing.SetActive(true);
    }
}
