using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] float angulaVelocity = 30f;
    float horizontalAngle = 0f;
    float verticalAngle = 0f;
    void Update()
    {
        var horizontalRotation = Input.GetAxis("Horizontal") * angulaVelocity * Time.deltaTime;
        var verticalRotaion = -Input.GetAxis("Vertical") * angulaVelocity * Time.deltaTime;

        horizontalAngle += horizontalRotation;

        verticalAngle += verticalRotaion;

        verticalAngle = Mathf.Clamp(verticalAngle, -80f, 80f);
        transform.rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
    }
}