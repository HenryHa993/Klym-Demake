using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothing : MonoBehaviour
{
    public GameObject Hat;
    public GameObject Shirt;
    public GameObject Bottom;

    public bool IsClothed;

    private int _numClothes;

    public void Wear(GameObject clothing)
    {
        clothing.SetActive(true);
        _numClothes++;

        if (_numClothes >= 3)
        {
            IsClothed = true;
        }
    }
}
