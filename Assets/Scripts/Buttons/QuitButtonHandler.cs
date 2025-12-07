using UnityEngine;
using UnityEngine.UI; // Required for UI components like Button

[RequireComponent(typeof(Button))] // Ensures this script is always on an object with a Button
public class QuitButtonHandler : MonoBehaviour
{
    void Start()
    {
        // Get the Button component attached to this GameObject
        Button quitButton = GetComponent<Button>();

        // Make sure we found the button
        if (quitButton != null)
        {
            // Clear any existing listeners to be safe
            quitButton.onClick.RemoveAllListeners();

            // Add a new listener that quits the app
            quitButton.onClick.AddListener(() => Application.Quit());
        }
    }
}