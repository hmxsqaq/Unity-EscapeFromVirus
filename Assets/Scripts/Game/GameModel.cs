using Framework;

namespace Game
{
    public class GameModel : Singleton<GameModel>
    {
        private int _life = 10;
        private int _score = 0;
        private int _minutes = 0;
        private int _seconds = 0;
        public int Life
        {
            get => _life;
            set
            {
                _life = value;
                EventManager.Instance.Trigger(EventNameHelper.OnLifeChange);
            }
        }

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                EventManager.Instance.Trigger(EventNameHelper.OnScoreChange);
            }
        }

        public int Minutes
        {
            get => _minutes;
            set
            {
                _minutes = value;
                EventManager.Instance.Trigger(EventNameHelper.OnMinuteChange);
            }
        }

        public int Seconds
        {
            get => _seconds; 
            set => _seconds = value;
        }
    }
}