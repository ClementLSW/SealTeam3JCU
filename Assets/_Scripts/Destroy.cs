using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float destroyAfter;
    private void Start()
    {
        Destroy(gameObject, destroyAfter);
    }
}
