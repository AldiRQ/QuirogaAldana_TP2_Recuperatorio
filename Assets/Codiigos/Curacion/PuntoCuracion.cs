using UnityEngine;

public class PuntoCuracion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerLives playerLives = other.GetComponent<PlayerLives>();

        if (playerLives != null)
        {
            playerLives.RestoreLives();
        }
    }
}
