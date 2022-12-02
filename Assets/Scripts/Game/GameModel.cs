using Framework;
using UI;

namespace Game
{
    public class GameModel : Singleton<GameModel>
    {
        private int _life;
        private int _score;
        private int _minutes;
        private int _seconds;

        public int BackgroundIndex;
        public int ProtagonistIndex;

        public int CurrentDamage;
        public int CurrentQuestionIndex;

        public DataRoot DataRoot;
        
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
                
            }
        }

        public int Seconds
        {
            get => _seconds; 
            set
            {
                _seconds = value;
            }
        }
    }
}