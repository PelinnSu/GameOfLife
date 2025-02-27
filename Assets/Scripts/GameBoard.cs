using UnityEngine;
using UnityEngine.Tilemaps;
public class GameBoard : MonoBehaviour
{
   [SerializeField] private Tilemap currentState;
   [SerializeField] private Tilemap nextState;
   [SerializeField] private Tile aliveTile;
   [SerializeField] private Tile deadTile;

   [SerializeField] private float updateInterval = 0.05f;

   [SerializeField] Vector2Int[] pattern;
}
