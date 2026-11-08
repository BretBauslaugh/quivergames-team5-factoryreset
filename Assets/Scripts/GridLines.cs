using UnityEngine;

public class GridLines : MonoBehaviour
{
    [SerializeField] private int halfExtentX = 10;
    [SerializeField] private int halfExtentZ = 10;

    [SerializeField] Color lineColor = new Color(1f, 1f, 1f, 0.35f);
    [SerializeField] float yOffset = 0.01f;

    private Grid _grid;

    void OnDrawGizmos()
    {
        if (_grid == null) _grid = GetComponent<Grid>();
        if (_grid == null) return;

        Gizmos.color = lineColor;

        var t = transform;
        var cs = _grid.cellSize;                    

        Vector3 originLocal = new Vector3(-halfExtentX * cs.x, 0f, -halfExtentZ * cs.z);
        Vector3 stepX = new Vector3(cs.x, 0f, 0f);
        Vector3 stepZ = new Vector3(0f, 0f, cs.z);
        Vector3 upOffset = t.up * yOffset;

        int cols = halfExtentX * 2;
        int rows = halfExtentZ * 2;

        for (int x = 0; x <= cols; x++)
        {
            Vector3 aL = originLocal + stepX * x;
            Vector3 bL = aL + stepZ * rows;

            Vector3 aW = t.TransformPoint(aL) + upOffset;
            Vector3 bW = t.TransformPoint(bL) + upOffset;

            Gizmos.DrawLine(aW, bW);
        }

        for (int z = 0; z <= rows; z++)
        {
            Vector3 aL = originLocal + stepZ * z;
            Vector3 bL = aL + stepX * cols;

            Vector3 aW = t.TransformPoint(aL) + upOffset;
            Vector3 bW = t.TransformPoint(bL) + upOffset;

            Gizmos.DrawLine(aW, bW);
        }
    }
}
