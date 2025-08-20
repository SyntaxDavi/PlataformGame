using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    [Header("Alvo")]
    [Tooltip("O objeto que a câmera deve seguir")]
    public Transform Target;

    [Header("Configurações")]
    [Tooltip("Quão suavemente a câmera vai seguir o alvo, valores menores são mais grudados")]
    public float smoothSpeed = 0.125f;
    [Tooltip("O deslocamento da câmera em relação ao alvo")]
    public Vector3 offset;

    [Header("Configurações de Zoom")]
    [Tooltip("O 'tamanho' da visão da câmera. Valores menores aproximam, valores maiores afastam.")]
    public float cameraSize = 5f;
    [Tooltip("Quão suavemente a câmera aplicará o zoom.")]
    public float zoomSmoothSpeed = 1.0f;

    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private Vector3 shakeOffset = Vector3.zero;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (Target == null) return;

        Vector3 desiredPosition = Target.position + offset;
        desiredPosition.z = transform.position.z;

        Vector3 smothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        transform.position = smothedPosition + shakeOffset;

        //Zoom

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cameraSize, zoomSmoothSpeed);
    }

    public void Shake(float duration, float magnitude)
    {
        if(shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f,1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}
