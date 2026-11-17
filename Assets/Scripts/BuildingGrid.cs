using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    private readonly Dictionary<Vector3Int, BuildingInstance> _cells = new();
    private static readonly Vector3Int[] Dirs = { new(1, 0, 0), new(-1, 0, 0), new(0, 0, 1), new(0, 0, -1) };

    public Vector3Int ToCell(Vector3 world)
    {
        var c = _grid.WorldToCell(world);
        c.y = 0;
        return c;
    }

    public bool IsFree(Vector3Int c) => !_cells.ContainsKey(c);
    public void Register(BuildingInstance b, Vector3Int c) => _cells[c] = b;
    public void Unregister(BuildingInstance b)
    {
        if (_cells.TryGetValue(b.Cell, out var cur) && cur == b)
        {
            _cells.Remove(b.Cell);
        }
    }
    public IEnumerable<BuildingInstance> Neighbours(Vector3Int c)
    {
        foreach (var d in Dirs)
            if (_cells.TryGetValue(c + d, out var b)) yield return b;
    }
}