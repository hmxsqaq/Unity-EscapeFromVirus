using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SceneManager : MonoBehaviour
    {
        public RawImage backImage;
        private float _fadeSpeed = 3f;
        private bool _sceneStarting = true;
        private bool _sceneEnding;
        private string _sceneName;
        private Scene _scene;
 
        void Start()
        {
            backImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            backImage.color = Color.black;
        }
 
        void Update()
        {
            if (_sceneStarting)
            {
                StartScene();
            }
            if (_sceneEnding)
            {
                EndScene(_sceneName);
            }
        }
        
        private void FadeToClear()
        {
            backImage.color = Color.Lerp(backImage.color, Color.clear, _fadeSpeed*Time.fixedDeltaTime);
        }
        private void FadeToBlack()
        {
            backImage.color = Color.Lerp(backImage.color, Color.black, _fadeSpeed * Time.fixedDeltaTime);
        }
        
        private void StartScene()
        {
            backImage.enabled = true;
            FadeToClear();
            if(backImage.color.a <= 0.05f)
            {
                backImage.color = Color.clear;
                backImage.enabled = false;
                _sceneStarting = false;
            }
            
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