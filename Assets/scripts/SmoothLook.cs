using UnityEngine;

public class SmoothLook : MonoBehaviour
{
    public float rotateSpeed = 120f;
    public float smoothTime = 10f;
    private float targetAngle = 0f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (h != 0)
        {
            targetAngle += h * rotateSpeed * Time.deltaTime;
        }

        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            smoothTime * Time.deltaTime
        );
    }
}
