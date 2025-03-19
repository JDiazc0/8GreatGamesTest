using UnityEngine;

public class InputManager : MonoBehaviour
{
    public LayerMask trolleyLayer;
    private TrolleyController selectedTrolley;
    private Vector2 touchStartPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, trolleyLayer))
            {
                selectedTrolley = hit.collider.GetComponentInParent<TrolleyController>();
                if (selectedTrolley != null)
                {
                    touchStartPosition = Input.mousePosition;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedTrolley != null)
        {
            Vector2 touchEndPosition = Input.mousePosition;
            Vector2 swipeDirection = touchEndPosition - touchStartPosition;

            if (swipeDirection.magnitude > 20)
            {
                Vector2Int newDirection = GetDirectionFromSwipe(swipeDirection);
                selectedTrolley.ChangeDirection(newDirection);
            }
            selectedTrolley = null;
        }
    }

    private Vector2Int GetDirectionFromSwipe(Vector2 swipe)
    {
        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            return swipe.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            return swipe.y > 0 ? Vector2Int.up : Vector2Int.down;
        }
    }
}