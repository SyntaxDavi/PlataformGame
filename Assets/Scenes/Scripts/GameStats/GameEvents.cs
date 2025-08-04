using UnityEngine;
using System;

// Uma classe estática funciona como um hub central para todos os eventos do jogo.
// Qualquer script pode disparar ou se inscrever a um evento sem precisar
// de uma referência direta a outros objetos.
public static class GameEvents 
{
    // -- EVENTOS DA RUN --
    public static event Action OnRunStarted;
    public static void TriggerRunStarted() => OnRunStarted?.Invoke();

    // Evento disparado quando o jogador chega ao final de uma sala e interage.
    public static event Action OnPlayerReachedRoomExit;
    public static void TriggerPlayerReachedRoomExit() => OnPlayerReachedRoomExit?.Invoke();

    // Evento disparado pelo RunManager quando uma nova sala é efetivamente carregada.
    // Podemos passar dados da sala se necessário no futuro, ex: Action<RoomData>
    public static event Action OnRoomLoaded;
    public static void TriggerRoomLoaded() => OnRoomLoaded?.Invoke();

    // -- EVENTOS DO JOGADOR --
    public static event Action OnPlayerDeath;
    public static void TriggerPlayerDeath() => OnPlayerDeath?.Invoke();

    public static event Action<float, float> OnPlayerHealthChanged;
    public static void TriggerPlayerHealthChanged(float currentHealth, float maxHealth) => OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

    // -- EVENTOS DE COMBATE --
    public static event Action<GameObject> OnEnemyDied;
    public static void TriggerEnemyDied(GameObject enemy) => OnEnemyDied?.Invoke(enemy);
}
