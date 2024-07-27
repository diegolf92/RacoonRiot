using UnityEngine;
using UnityEngine.UI;

public class BarraHambre: MonoBehaviour
{
    public Scrollbar scrollbar; // El Scrollbar UI
    public float duration = 10f; // Duraci�n en segundos para que el Scrollbar se vac�e

    private float timeLeft;

    void Start()
    {
        // Inicializar el tiempo restante al inicio
        timeLeft = duration;
    }

    void Update()
    {
        // Disminuir el tiempo restante
        timeLeft -= Time.deltaTime;

        // Calcular el valor del Scrollbar
        float value = Mathf.Clamp01(timeLeft / duration);

        // Asignar el valor al Scrollbar
        scrollbar.size = value;

        // Comprobar si el tiempo ha llegado a cero
        if (timeLeft <= 0f)
        {
            // Realizar alguna acci�n cuando el tiempo se agote
            OnTimeOut();
        }
    }

    private void OnTimeOut()
    {
        // Aqu� puedes definir qu� pasa cuando el Scrollbar se vac�a
        Debug.Log("El tiempo se ha agotado.");
        // Por ejemplo, podr�as reiniciar el tiempo
        // timeLeft = duration;
    }
}
