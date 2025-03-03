using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void mostrarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void salirDeJuego()
    {
        Application.Quit();
    }
}
