using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Alvo")]
    [Tooltip("O objeto que a câmera deve seguir")]
    public Transform Target;

    [Header("Configurações")]
    [Tooltip("Quão suavemente a câmera vai seguir o alvo, valores menores são mais grudados")]
    public float smoothSpeed = 0.125f;

    [Tooltip("O deslocamento da câmera em relação ao alvo")]
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
