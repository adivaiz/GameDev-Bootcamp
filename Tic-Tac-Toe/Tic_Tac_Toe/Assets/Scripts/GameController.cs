using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Class that stores data for each player (panel, text, button)
[System.Serializable]
public class Player
{
    public Image panel;     // UI panel showing player's color
    public TMP_Text text;   // UI text showing player's label (X or O)
    public Button button;   // Button to choose this player at start
}

// Class that stores colors for active and inactive states
[System.Serializable]
public class PlayerColor
{
    public Color panelColor;  // Panel color
    public Color textColor;   // Text color
}

public class GameController : MonoBehaviour
{
    public TMP_Text[] buttonList;     // Array of all grid cells (buttons text)
    private string playerSide;        // Current player's side ("X" or "O")
    public GameObject gameOverPanel;  // Panel that shows at game over
    public TMP_Text gameOverText;     // Text that shows win/draw message
    private int moveCount;            // Number of moves made so far
    public GameObject restartButton;  // Restart button
    public GameObject startInfo;      // Start screen info panel
    public Player playerx;            // Player X settings
    public Player playerO;            // Player O settings
    public PlayerColor activePlayerColor;   // Colors for active player
    public PlayerColor inactivePlayerColor; // Colors for inactive player

    void Awake()
    {
        gameOverPanel.SetActive(false);   // Hide game over panel at start
        SetGameControllerReferenceOnButtons(); // Link all grid cells to controller
        moveCount = 0;                    // Reset moves
        restartButton.SetActive(false);   // Hide restart button at start
    }

    // Link each grid button to the GameController
    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    // Set which side starts ("X" or "O")
    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
            SetPlayerColors(playerx, playerO);  // Highlight player X
        else
            SetPlayerColors(playerO, playerx);  // Highlight player O

        StartGame();
    }

    // Starts the game (enable grid, disable start screen)
    void StartGame()
    {
        SetBoardInteractable(true);   // Allow clicking grid
        SetPlayerButtons(false);      // Disable player select buttons
        startInfo.SetActive(false);   // Hide start info
    }

    // Return the current player's side
    public string GetPlayerSide()
    {
        return playerSide;
    }

    // Called when a player makes a move
    public void EndTurn()
    {
        moveCount++;

        // --- Check all win conditions ---
        // Horizontal rows
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
            GameOver(playerSide);
        else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
            GameOver(playerSide);
        else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
            GameOver(playerSide);

        // Vertical columns
        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
            GameOver(playerSide);
        else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
            GameOver(playerSide);
        else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
            GameOver(playerSide);

        // Diagonals
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
            GameOver(playerSide);
        else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
            GameOver(playerSide);

        // Draw (no winner after 9 moves)
        else if (moveCount >= 9)
            GameOver("draw");

        // Otherwise, continue the game and switch sides
        else
            ChangeSides();
    }

    // Ends the game and shows the winner/draw
    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false);   // Stop further clicks
        if (winningPlayer == "draw")
        {
            SetGameOverText("It's a draw!");
            SetPlayerColorsInactive();
        }
        else
        {
            SetGameOverText(winningPlayer + " Wins!");
        }
        restartButton.SetActive(true); // Show restart button
    }

    // Switch to the other player
    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";  // Toggle between X and O

        if (playerSide == "X")
            SetPlayerColors(playerx, playerO);
        else
            SetPlayerColors(playerO, playerx);
    }

    // Display message on game over panel
    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    // Restart the game
    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        startInfo.SetActive(true);        // Show start screen again
        SetPlayerButtons(true);           // Allow choosing starting side again
        SetPlayerColorsInactive();        // Reset colors
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";      // Clear the grid
        }
        restartButton.SetActive(false);
    }

    // Enable or disable the grid buttons
    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    // Highlight the active player and dim the inactive player
    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    // Enable/disable the "choose player" buttons
    void SetPlayerButtons(bool toggle)
    {
        playerx.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    // Reset both players to inactive colors
    void SetPlayerColorsInactive()
    {
        playerx.panel.color = inactivePlayerColor.panelColor;
        playerx.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }
}