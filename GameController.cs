using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.GetComponent<PlayerMovement>().finish){
            GetComponent<Timer>().UpdateTimer();
        }
    }
}
