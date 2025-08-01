using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class RoomExit : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("Arraste o Prefab do Canvas de Interação aqui")]
    public GameObject InteractionCanvas;

    private Canvas canvasComponent;
    private bool playerInRange = false;
    private void Awake()
    {
        if(InteractionCanvas != null)
        {
            canvasComponent = InteractionCanvas.GetComponent<Canvas> ();
            if(canvasComponent == null)
            {
                Debug.LogError("O GameObject de interação não tem um componente Canvas", this);
                return;
            }
        }

        if(canvasComponent.renderMode == RenderMode.WorldSpace && canvasComponent == null)
        {
            canvasComponent.worldCamera = Camera.main;
        }

        InteractionCanvas.SetActive(false);
    }
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            //Desativa a UI antes de mudar de sala para não ficar visível por um frame

            if(InteractionCanvas != null) InteractionCanvas.SetActive(false);

            Debug.Log("E pressinado, notificando o runmangaer");
            RunManager.Instance.PlayerReachedEndOfRoom();
            playerInRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if(InteractionCanvas != null) InteractionCanvas.SetActive(true);
            Debug.Log("Jogador entrou na area de saida, pressione E para continuar. ");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InteractionCanvas != null)
            {
                InteractionCanvas.SetActive(false);
            }
            playerInRange = false;
        }
    }
}
