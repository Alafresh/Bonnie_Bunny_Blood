using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string sceneName; // Nombre de la escena a cargar

    public void LoadNewScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        // Inicia la carga de la escena de forma asíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Evita que la escena se active automáticamente al terminar de cargar
        asyncLoad.allowSceneActivation = false;

        // Monitorea el progreso
        while (!asyncLoad.isDone)
        {
            Debug.Log($"Progreso de carga: {asyncLoad.progress * 100}%");

            // Cuando la carga alcanza el 90% (casi lista)
            if (asyncLoad.progress >= 0.9f)
            {
                    asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}