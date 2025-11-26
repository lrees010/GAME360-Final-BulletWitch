using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for UI components like Button

[RequireComponent(typeof(Button))] // Ensures this script is always on an object with a Button
public class PlayButtonHandler : MonoBehaviour
{
    void Start()
    {
        // Get the Button component attached to this GameObject
        Button playButton = GetComponent<Button>();

        // Make sure we found the button
        if (playButton != null)
        {

            // Clear any existing listeners to be safe
            playButton.onClick.RemoveAllListeners();

            // Add a new listener that calls the Clicky method
            playButton.onClick.AddListener(Clicky);
        }
    }

    void Clicky()
    {
        Debug.Log("click");
        GameManager.Instance.StartGame();
        SceneManager.LoadScene("MainGame");
    }
}