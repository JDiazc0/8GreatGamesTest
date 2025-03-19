using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class TrolleyController : MonoBehaviour
{
    public float moveDuration = 0.2f;
    public LayerMask obstacleLayer;
    public Grid grid;

    private Vector2Int direction = Vector2Int.right;
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
    }

    public void ChangeDirection(Vector2Int newDirection)
    {
        if (newDirection != -direction)
        {
            direction = newDirection;

            head.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
            partRotations[0] = head.rotation;

            MoveTrolley();

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
    }

    private void MoveTrolley()
    {
        Vector3Int currentCell = grid.WorldToCell(head.position);
        Vector3Int newCell = currentCell + new Vector3Int(direction.x, 0, direction.y);

        Vector3 newPosition = grid.CellToWorld(newCell);

        if (IsPositionValid(newPosition))
        {

            for (int i = trolleyParts.Length - 1; i > 0; i--)
            {
                trolleyParts[i].position = trolleyParts[i - 1].position;
                partRotations[i] = partRotations[i - 1];
            }

            head.DOMove(newPosition, moveDuration);
        }
        else return;
    }

    private bool IsPositionValid(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f, obstacleLayer);
        return colliders.Length == 0;
    }
}