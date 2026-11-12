using UnityEngine;
using System.Collections;


public class PuntoCuracion : MonoBehaviour
{
    [Header("Configuracion")]
    public float tiempoEntreCuraciones = 1.5f;
    public float tiempoRecarga = 10f;

    private bool disponible = true;
    private bool jugadorDentro = false;

    private PlayerLives playerLives;
    private Coroutine curacionCoroutine;

    private Renderer[] renderers;
    private Collider[] colliders;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!disponible)
            return;

        PlayerLives lives = other.GetComponent<PlayerLives>();

        if (lives == null)
            return;

        if (lives.GetLives() >= 3)
            return;

        jugadorDentro = true;
        playerLives = lives;

        if (curacionCoroutine == null)
            curacionCoroutine = StartCoroutine(CurarJugador());
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerLives == null)
            return;

        if (other.gameObject == playerLives.gameObject)
        {
            jugadorDentro = false;
            playerLives = null;
        }
    }

    private IEnumerator CurarJugador()
    {
        while (jugadorDentro && disponible)
        {
            yield return new WaitForSeconds(tiempoEntreCuraciones);

            if (!jugadorDentro || playerLives == null)
                break;

            if (playerLives.GetLives() >= 3)
                break;

            playerLives.RestoreOneLife();

            if (playerLives.GetLives() >= 3)
                break;
        }

        if (jugadorDentro && playerLives != null &&
            playerLives.GetLives() < 3)
        {
            curacionCoroutine = null;
            yield break;
        }

        curacionCoroutine = null;

        if (disponible)
            StartCoroutine(RecargarEstacion());
    }

    private IEnumerator RecargarEstacion()
    {
        disponible = false;

        MostrarEstacion(false);

        yield return new WaitForSeconds(tiempoRecarga);

        MostrarEstacion(true);

        disponible = true;
    }

    private void MostrarEstacion(bool mostrar)
    {
        foreach (Renderer r in renderers)
            r.enabled = mostrar;

        foreach (Collider c in colliders)
            c.enabled = mostrar;
    }
}
