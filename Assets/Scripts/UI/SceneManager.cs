using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager _instance;
        public string[] scenesName;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        public void SceneSwitch(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scenesName[sceneIndex]);
        }
    }
}