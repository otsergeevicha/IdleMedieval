using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoading : MonoBehaviour
{
    [SerializeField] private int sceneID;
    [SerializeField] private Image sceneProgress;

    void Start()
    {
        StartCoroutine(AsyncLoad());
    }

   IEnumerator AsyncLoad()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        while (!operation.isDone)
        {
            float progress = operation.progress / .9f;
            sceneProgress.fillAmount = progress;
            yield return null;
        }
    }
}
