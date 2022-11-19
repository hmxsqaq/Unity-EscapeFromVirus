using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SceneManager : MonoBehaviour
    {
        public RawImage backImage;
        public float fadeSpeed = 1.5f;
        private bool _sceneEnding = false;
        private string _sceneName;
 
        void Start()
        {
            backImage.enabled = false;
            backImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        }
 
        void Update()
        {
            if (_sceneEnding)
            {
                EndScene(_sceneName);
            }
        }
        
        private void FadeToBlack()
        {
            backImage.color = Color.Lerp(backImage.color, Color.black, fadeSpeed * Time.deltaTime);
        }
        
        private void EndScene(string sceneName)
        {
            backImage.enabled = true;
            FadeToBlack();
            if(backImage.color.a >= 0.95f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                _sceneEnding = false;
            }
        }

        public void SceneSwitch(string sceneName)
        {
            _sceneName = sceneName;
            _sceneEnding = true;
        }
    }
}