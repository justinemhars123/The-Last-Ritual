using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Hide and lock cursor when game starts
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Press Escape to show cursor again
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}