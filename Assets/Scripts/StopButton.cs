using UnityEngine;

public class StopButton : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;

    public void StopSimulation()
    {
        gameBoard.StopSimulation();
    }
}
