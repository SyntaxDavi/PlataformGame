using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class RoomExit : MonoBehaviour
{
    private bool playerInRange = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Jogador entrou na area de saida, pressione E para continuar. ");
        }

        //Mostrar UI
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Jogador entrou na area de saida, pressione E para continuar. ");
        }
        //Esconder UI
    }

    private void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            playerInRange = false;
            Debug.Log("E pressinado, notificando o runmangaer");
            RunManager.Instance.PlayerReachedEndOfRoom();
        }
    }
}
