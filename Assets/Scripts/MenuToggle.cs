using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject MainMenu;  // Drag your menu panel here in the Inspector

    private bool isMenuVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuVisible = !isMenuVisible;
            MainMenu.SetActive(isMenuVisible);
        }
    }
}