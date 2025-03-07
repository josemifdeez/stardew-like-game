using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AdjustGlobalLight : MonoBehaviour
{
    [SerializeField] Color caveLightColor = new Color(0.1f, 0.1f, 0.1f); // Color más oscuro
    [SerializeField] float caveLightIntensity = 0.2f; // Intensidad más baja

    private Light2D globalLight;
    private Color originalLightColor;
    private float originalLightIntensity;

    void Start()
    {
        // Encuentra el componente Global Light 2D en la escena
        globalLight = FindObjectOfType<Light2D>();

        if (globalLight != null && globalLight.lightType == Light2D.LightType.Global)
        {
            // Guarda los parámetros originales
            originalLightColor = globalLight.color;
            originalLightIntensity = globalLight.intensity;

            // Cambia los parámetros de la luz para la cueva
            globalLight.color = caveLightColor;
            globalLight.intensity = caveLightIntensity;
        }
        else
        {
            Debug.LogError("No se encontró una luz global 2D en la escena.");
        }
    }

    void OnDestroy()
    {
        if (globalLight != null)
        {
            // Restaura los parámetros originales de la luz
            globalLight.color = originalLightColor;
            globalLight.intensity = originalLightIntensity;
        }
    }
}
