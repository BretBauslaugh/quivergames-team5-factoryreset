using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    private readonly Vector2Int _areaMin = new(-4, -3);
    private readonly Vector2Int _areaSize = new(7, 7);
    private readonly Dictionary<Vector3Int, BuildingInstance> _cells = new();
    private static readonly Vector3Int[] Dirs = { new(1, 0, 0), new(-1, 0, 0), new(0, 0, 1), new(0, 0, -1) };

    public Vector3Int ToCell(Vector3 world)
    {
        var c = _grid.WorldToCell(world);
        c.y = 0;
        return c;
    }

    public bool IsRowFull(int z)
    {
        if (z < _areaMin.y || z >= _areaMin.y + _areaSize.y) return false;
        for (int x = _areaMin.x; x < _areaMin.x + _areaSize.x; x++)
            if (!_cells.ContainsKey(new Vector3Int(x, 0, z))) return false;
        return true;
    }

    public bool IsColumnFull(int x)
    {
        if (x < _areaMin.x || x >= _areaMin.x + _areaSize.x) return false;
        for (int z = _areaMin.y; z < _areaMin.y + _areaSize.y; z++)
            if (!_cells.ContainsKey(new Vector3Int(x, 0, z))) return false;
        return true;
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
    public IEnumerable<BuildingInstance> Row(int z)
    {
        for (int x = _areaMin.x; x < _areaMin.x + _areaSize.x; x++)
            if (_cells.TryGetValue(new Vector3Int(x, 0, z), out var b)) yield return b;
    }
    public IEnumerable<BuildingInstance> Column(int x)
    {
        for (int z = _areaMin.y; z < _areaMin.y + _areaSize.y; z++)
            if (_cells.TryGetValue(new Vector3Int(x, 0, z), out var b)) yield return b;
    }
}