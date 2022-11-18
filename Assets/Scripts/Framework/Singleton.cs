namespace Framework
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.OnInstanceCreate();
                }

                return _instance;
            }
        }

        protected virtual void OnInstanceCreate()
        {
        }
    }
}