using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private void Update()
    {
        // If the Esc key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the end panel
            uiManager.ToggleEndPanel();
        }
    }
}
