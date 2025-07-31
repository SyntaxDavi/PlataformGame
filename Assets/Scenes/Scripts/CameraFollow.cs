using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Alvo")]
    [Tooltip("O objeto que a c�mera deve seguir")]
    public Transform Target;

    [Header("Configura��es")]
    [Tooltip("Qu�o suavemente a c�mera vai seguir o alvo, valores menores s�o mais grudados")]
    public float smoothSpeed = 0.125f;

    [Tooltip("O deslocamento da c�mera em rela��o ao alvo")]
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (Target == null) return;

        Vector3 desiredPosition = Target.position + offset;

        desiredPosition.z = transform.position.z;

        Vector3 smothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        transform.position = smothedPosition;
    }
}
