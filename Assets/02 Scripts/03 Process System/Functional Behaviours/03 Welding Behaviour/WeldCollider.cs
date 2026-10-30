using UnityEngine;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class WeldCollider : MonoBehaviour
    {
        WeldingBehaviour _welding;
        public GameObject WeldingHolder;
        private Collider WeldingCollider;
        private bool initialized=false;

        SimpleLoading LoadingBar;

        [SerializeField] private float targetTime = 0f;
        private float timer;

        public void Start()
        {
            targetTime = 0;
            gameObject.MustHaveComponent<SimpleLoading>();
            _welding = gameObject.transform.parent.parent.gameObject.GetComponent<WeldingBehaviour>();
            LoadingBar = gameObject.GetComponent<SimpleLoading>();

            WeldingHolder = GameObject.Find(GLV.WeldingName);
            if (WeldingHolder==null)
            {
                Debug.LogError("Missing Holder");
            }
        }

        private void OnEnable()
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }

        private void OnDisable()
        {
            gameObject.GetComponent<Collider>().enabled = false;
            _welding.spark.SetActive(false);

        }
        void OnTriggerEnter(Collider c)
        {
            if (c.gameObject == WeldingHolder)
            {
                _welding.spark.SetActive(true);
                timer = Time.time;
            }

        }
        private void OnTriggerStay(Collider c)
        {
            if (c.gameObject == WeldingHolder)
            {
                targetTime += Time.time - timer;
                timer = Time.time;
                LoadingBar.call(targetTime);
            }
            if (targetTime>= 4.0f)
            {
                _welding.ReleseTrigger(gameObject);
            }
        }
        void OnTriggerExit(Collider c)
        {
            if (c.gameObject == WeldingHolder)
            {
                _welding.spark.SetActive(false);
            }
        }
    }
}