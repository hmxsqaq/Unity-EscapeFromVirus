using UnityEngine;

namespace Game
{
    public class Element : MonoBehaviour
    {
        private bool _del;
        private Vector2 _direction;
        private GameObject _protagonist;
        public float elementSpeed;
        public int damage;

        private void Awake()
        {
            _del = false;
        }

        private void Start()
        {
            _protagonist = GameObject.FindWithTag("Protagonist");
            _direction = _protagonist.transform.position - transform.position;
        }

        private void Update()
        {
            transform.Translate(elementSpeed * Time.deltaTime * _direction);
        }

        private void OnBecameVisible()
        {
            _del = true;
        }

        private void OnBecameInvisible()
        {
            if(_del) Destroy(gameObject);
        }
    }
}