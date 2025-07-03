using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject menuPanel;  // Drag your menu panel here in the Inspector

    private bool isMenuVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuVisible = !isMenuVisible;
            menuPanel.SetActive(isMenuVisible);
        }
    }
}