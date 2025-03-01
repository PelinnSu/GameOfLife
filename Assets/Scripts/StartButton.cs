using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;

   public void StartSimulation()
    {
        gameBoard.StartCoroutine(gameBoard.Simulate());
    }
}
