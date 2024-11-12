using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glove : MonoBehaviour
{

    public GameObject hand;
    public bool left;
    private bool isBlocking;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = hand.transform.position + Vector3.forward * 0.2f;
        if(left){
            gameObject.transform.position += Vector3.right * 0.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if()
    if (Vector3.Dot(hand.transform.up, Vector3.up) > 0.9f)
    {
        isBlocking = true;
    }
    else
    {
        isBlocking = false;
    }
    }
}
