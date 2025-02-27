using UnityEngine;

[CreateAssetMenu(menuName = "GameOfLife/Pattern")]
public class Pattern: ScriptableObject
{
    public Vector2Int[] cells;
}
