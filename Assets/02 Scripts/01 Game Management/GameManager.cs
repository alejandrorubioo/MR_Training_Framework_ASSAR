using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


namespace MR_Training_ASSAR.GameManagement
{
    public class GameManager : Singleton<GameManager>
    {
        public static int ShowcaseNumber { get; set; } = 1;
        public static int LanguageTag { get; set; } = 0;
        private DateTime _sessionStartTime;
        private DateTime _sessionEndTime;
        public int numberOfShowcase = 0;
        private float _target;

        private bool loaderScreenReady;
        public bool exit = false;
        public bool calibrating=false;
        public static GameManager gameManager;

        [SerializeField] private GameObject _loaderCanvas;
        [SerializeField] private GameObject _varjoCamera;
        [SerializeField] private GameObject _scene;
        //[SerializeField] private Image _progressBar;
        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            _loaderCanvas = GameObject.Find("_loaderScreen");
            ShowcaseNumber = SceneManager.GetActiveScene().buildIndex;
            // Do something else
            gameManager = this;
        }
        private void Update()
        {
            if (exit)
            {
                QuitShowcase();
                exit = false;
            }
            if(calibrating)
            {
                if(_varjoCamera.transform.localPosition.y>-0.9)
                {
                    calibrating = false;
                    _scene.SetActive(true);
                    _loaderCanvas.SetActive(false);
                }
            }
        }

        void Start()
        {
            _sessionStartTime = DateTime.Now;
            Debug.Log("Game session start @: " + DateTime.Now);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneLoaded();
        }

        void OnApplicationQuit()
        {
            _sessionEndTime = DateTime.Now;

            TimeSpan timeDifference = _sessionEndTime.Subtract(_sessionStartTime);

            Debug.Log("Game session ended @: " + DateTime.Now);
            Debug.Log("Game session lasted: " + timeDifference);
        }


        public void GetLanguage(int _languageTag)
        {
            LanguageTag = _languageTag;
        }

        public async void ChangeShowcase(int _SceneIndex)
        {

            _target = 0;
            //_progressBar.fillAmount = 0;
            ShowcaseNumber = _SceneIndex;
            var scene = SceneManager.LoadSceneAsync(ShowcaseNumber);
            scene.allowSceneActivation = false;
            if (loaderScreenReady)
            {
                _loaderCanvas.SetActive(true);
                _scene.SetActive(false);

                do
                {
                    await Task.Delay(2000);

                    //_progressBar.fillAmount = scene.progress;
                } while (scene.progress < 0.9f);

                await Task.Delay(1000);
                _loaderCanvas.SetActive(false);
                scene.allowSceneActivation = true;
                
            }
            else
            {
                SceneManager.LoadScene(ShowcaseNumber);
                
            }
            SceneLoaded();
        }
        private void SceneLoaded()
        { 
            _loaderCanvas = GameObject.Find("_loaderScreen");
            _scene = GameObject.Find("Scene");
            _varjoCamera = GameObject.Find("Varjo Camera");
            if (_loaderCanvas == null || _scene == null)
            {
                loaderScreenReady = false;
                Debug.Log("Loading Screen not Found");
            }
            else
            { 
                loaderScreenReady = true;
                Debug.Log("New Loading Screen Found");
                if(_varjoCamera.transform.localPosition.y<=-0.8)
                {
                    calibrating = true;
                    _loaderCanvas.SetActive(true);
                    _scene.SetActive(false);
                }
                else
                {
                    _loaderCanvas.SetActive(false);
                    _scene.SetActive(true);
                }
            }
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneLoaded();
            Debug.Log("sceneLoaded");
        }

        public void ResetShowcase()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public async void QuitShowcase()
        {
            var scene = SceneManager.LoadSceneAsync(0);
            scene.allowSceneActivation = false;
            if (loaderScreenReady)
            {
                _loaderCanvas.SetActive(true);
                _scene.SetActive(false);

                do
                {
                    await Task.Delay(2000);

                    //_progressBar.fillAmount = scene.progress;
                } while (scene.progress < 0.9f);

                await Task.Delay(1000);

                scene.allowSceneActivation = true;
                _loaderCanvas.SetActive(false);
            }
            else
            {
                SceneManager.LoadScene(ShowcaseNumber);
            }
            SceneLoaded();
        }
    }
}
