using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSpace : MonoBehaviour
{
    // The Button component attached to this grid space
    public Button button;

    // The Text element inside the button that shows "X" or "O"
    public TMP_Text buttonText;

    // Reference to the GameController (the "manager" of the game)
    private GameController gameController;

    // Called when this grid space is clicked by a player
    public void SetSpace()
    {
        // Write the current player's symbol (X or O) in the button
        buttonText.text = gameController.GetPlayerSide();

        // Disable the button so it cannot be clicked again
        button.interactable = false;

        // Notify the GameController that the turn has ended
        gameController.EndTurn();
    }

    // Called by GameController to connect this grid space to it
    public void SetGameControllerReference(GameController controller)
    {
        // Save the reference to the GameController
        gameController = controller;
    }
}