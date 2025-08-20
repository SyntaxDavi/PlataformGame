using UnityEngine;

public class TiroExplosionEffect : MonoBehaviour
{
   public void OnAnimationFinished()
    {
        gameObject.SetActive(false);
    }
}
