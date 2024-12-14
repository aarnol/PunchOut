using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glove : MonoBehaviour
{

    public GameObject hand;
    public bool left;
   
   
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = hand.transform.position + Vector3.forward * 0.2f;
        if(left){
            gameObject.transform.position += Vector3.left * 0.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
