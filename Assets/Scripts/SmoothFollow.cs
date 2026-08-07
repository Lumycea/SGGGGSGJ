using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float smoothness = 0.125f;

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 position2D = Vector2.Lerp(transform.position, target.position, smoothness);
            transform.position = new Vector3(position2D.x, position2D.y, transform.position.z);
        }
    }
}
