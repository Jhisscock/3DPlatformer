using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperTools : MonoBehaviour
{
    public GameObject player, respawn;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = respawn.transform.position;
            player.GetComponent<CharacterController>().enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(0f,-0.5f,152f);
            player.GetComponent<CharacterController>().enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(-0.18026416f,-52.0027008f,306.084534f);
            player.GetComponent<CharacterController>().enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.Alpha3)){
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(0.0f,-55.17255401611328f,487.0133361816406f);
            player.GetComponent<CharacterController>().enabled = true;
        }        
    }
}
