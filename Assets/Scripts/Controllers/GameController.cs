// GameController.cs
// Handles global scene management.
// Should only be accessed by the specific controller for each scene.
// For example, in the Combat scene, only CombatController.cs should access this instance.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public GameState currentState { get; private set; }

    private void Awake()
    {
        currentState = GameState.Title;
        DontDestroyOnLoad(gameObject);
    }

    // TODO: Create game scenes.
    // Title and PrepareTeam scenes don't exist.
    // For now, create an empty scene and just implement UI for scene switching.
    public enum GameState
    {
        Title,
        Farm,
        PrepareTeam,
        Combat,
        Trade
    }

    // TODO: Switch scenes in here.
    public void SwitchGameState(int stateIndex)
    {
        currentState = (GameState)stateIndex;
        switch(currentState)
        {
            case GameState.Title:
                Debug.Log("Entering Title Screen");
                SceneManager.LoadScene(0);
                break;
            case GameState.Farm:
                Debug.Log("Entering Farm");
                SceneManager.LoadScene(1);
                break;
            case GameState.PrepareTeam:
                Debug.Log("Entering Combat Prep");
                SceneManager.LoadScene(2);
                break;
            case GameState.Combat:
                Debug.Log("Entering Combat");
                SceneManager.LoadScene(1);
                break;
            case GameState.Trade:
                Debug.Log("Entering Trade");
                SceneManager.LoadScene(4);
                break;
        }
    }

}
