using UnityEngine;

public class interactionController : MonoBehaviour
{
    public PlayerSpawner PSpawner;
    public PlataformSpawner PlataformSpawner;
    public GameObject InteractionCanvas;
    private bool PlayerInRange = false;

    private void Start()
    {
        if (InteractionCanvas != null)
        {
            InteractionCanvas.SetActive(false);
        }   
    }

    private void Update()
    {
        if(PlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player interagiu com o objeto!");
            InteractionCanvas.SetActive(false);

            PSpawner.ResetPosition();
            // PlataformSpawner.ReorganizePlataforms();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInRange = true;
            InteractionCanvas.SetActive(true);

           // Debug.Log("Player range true");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInRange = false;
            InteractionCanvas.SetActive(false);
           // Debug.Log("Player range false");
        }
    }
   
}
