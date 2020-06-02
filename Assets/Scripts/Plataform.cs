using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour
{
    public Vector3 initialPosition;
    private bool visibility;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        visibility = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!visibility)
        {
            GameManager.Instance.InstantiatePlataforms();
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() 
    {
        visibility = false;
    }

    private void OnBecameVisible() 
    {
        visibility = true;
    }
}
