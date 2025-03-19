using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class TrolleyController : MonoBehaviour
{
    public float moveDuration = 0.2f;
    public LayerMask obstacleLayer;
    public Grid grid;

    private Vector2Int direction;
    private Transform[] trolleyParts;
    private Transform head;
    private List<Quaternion> partRotations;

    void Start()
    {
        trolleyParts = new Transform[transform.childCount];
        partRotations = new List<Quaternion>();

        for (int i = 0; i < transform.childCount; i++)
        {
            trolleyParts[i] = transform.GetChild(i);
            partRotations.Add(trolleyParts[i].rotation);
        }

        head = trolleyParts[0];
        direction = GetInitialDirection();
    }

    private Vector2Int GetInitialDirection()
    {
        Vector3 forward = head.forward;

        if (Vector3.Dot(forward, Vector3.forward) > 0.5f) return Vector2Int.up;
        if (Vector3.Dot(forward, Vector3.back) > 0.5f) return Vector2Int.down;
        if (Vector3.Dot(forward, Vector3.right) > 0.5f) return Vector2Int.right;
        if (Vector3.Dot(forward, Vector3.left) > 0.5f) return Vector2Int.left;

        return Vector2Int.right;
    }


    public void ChangeDirection(Vector2Int newDirection)
    {
        if (newDirection != -direction)
        {
            Vector3Int currentCell = grid.WorldToCell(head.position);
            Vector3Int newCell = currentCell + new Vector3Int(newDirection.x, 0, newDirection.y);
            Vector3 newPosition = grid.CellToWorld(newCell);

            if (IsPositionValid(newPosition))
            {
                direction = newDirection;

                head.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
                partRotations[0] = head.rotation;

                // Move trolley parts
                for (int i = trolleyParts.Length - 1; i > 0; i--)
                {
                    trolleyParts[i].position = trolleyParts[i - 1].position;
                    partRotations[i] = partRotations[i - 1];
                }

                head.DOMove(newPosition, moveDuration);

                // Rotate trolley parts
                for (int i = 1; i < trolleyParts.Length; i++)
                {
                    if (i == 1)
                    {
                        trolleyParts[i].rotation = head.rotation;
                        partRotations[i] = head.rotation;
                    }
                    else
                    {
                        trolleyParts[i].rotation = partRotations[i];
                    }
                }
            }
            else return;
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f, obstacleLayer);
        return colliders.Length == 0;
    }
}