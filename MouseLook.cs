using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public GameObject playerBody;
    [SerializeField]
    private Vector2 rotation = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -10f, 10f);
        playerBody.transform.eulerAngles = new Vector2(0, rotation.y) * mouseSensitivity;
        Camera.main.transform.localRotation = Quaternion.Euler(rotation.x * mouseSensitivity, 0, 0);
    }
    
}
