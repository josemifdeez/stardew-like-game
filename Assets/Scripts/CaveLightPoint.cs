using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LightManager : MonoBehaviour
{
    [SerializeField] string caveSceneName = "CaveScene"; // Nombre de la escena de la cueva
    [SerializeField] Color lightColor = Color.white; // Color de la luz
    [SerializeField] float lightIntensity = 1f; // Intensidad de la luz
    [SerializeField] float lightRadius = 5f; // Radio de la luz

    private GameObject playerLight;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == caveSceneName)
        {
            StartCoroutine(WaitAndAddLightToPlayer());
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == caveSceneName)
        {
            RemoveLightFromPlayer();
        }
    }

    private IEnumerator WaitAndAddLightToPlayer()
    {
        // Esperar hasta el final del frame para asegurar que el jugador esté inicializado
        yield return new WaitForEndOfFrame();

        // Intentar obtener la referencia al jugador nuevamente
        GameObject player = GameManager.instance?.player;

        if (player == null)
        {
            Debug.LogError("Player not found!");
            yield break;
        }

        // Crear un nuevo GameObject para la luz
        playerLight = new GameObject("PlayerLight");

        // Añadir el componente Light2D
        Light2D pointLight = playerLight.AddComponent<Light2D>();

        // Configurar los parámetros de la luz
        pointLight.lightType = Light2D.LightType.Point;
        pointLight.color = lightColor;
        pointLight.intensity = lightIntensity;
        pointLight.pointLightOuterRadius = lightRadius;

        // Hacer que la luz sea un hijo del jugador
        playerLight.transform.SetParent(player.transform);

        // Opcional: Asegurar que la luz está en la misma posición que el jugador
        playerLight.transform.localPosition = Vector3.zero;
    }

    private void RemoveLightFromPlayer()
    {
        if (playerLight != null)
        {
            Destroy(playerLight);
            playerLight = null;
        }
    }
}
