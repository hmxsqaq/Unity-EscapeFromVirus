using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<string, Delegate> _eventDic = new Dictionary<string, Delegate>();

        private void CheckAddingEvent(string eventType, Delegate action)
        {
            if (!_eventDic.ContainsKey(eventType))
            {
                _eventDic.Add(eventType,null);
            }

            Delegate tempDel = _eventDic[eventType];
            if (tempDel != null && tempDel.GetType() != action.GetType())
            {
                throw new Exception(
                    $"EventManager Error:try to add incorrect eventType {eventType},needed listener type is {tempDel.GetType()},given listener type is {action.GetType()}");
            }
        }

        private bool CheckRemovingEvent(string eventType, Delegate action)
        {
            bool result = false;
            if (!_eventDic.ContainsKey(eventType))
            {
                result = false;
            }
            else
            {
                Delegate tempDel = _eventDic[eventType];
                if (tempDel != null && tempDel.GetType() != action.GetType())
                {
                    throw new Exception(
                        $"EventManager Error:try to remove incorrect eventType {eventType},needed listener type is {tempDel.GetType()},given listener type is {action.GetType()}");
                }
                result = true;
            }
            return result;
        }

        public void AddEventListener(string eventType,Action action)
        {
            CheckAddingEvent(eventType,action);
            _eventDic[eventType] = (Action)Delegate.Combine((Action)_eventDic[eventType], action);
        }

        public void RemoveEventListener(string eventType, Action action)
        {
            if (CheckRemovingEvent(eventType,action))
            {
                _eventDic[eventType] = (Action)Delegate.Remove((Action)_eventDic[eventType], action);
            }
        }

        public void Trigger(string eventType)
        {
            if (_eventDic.TryGetValue(eventType,out var target))
            {
                if (target == null)
                    return;

                Delegate[] invocationList = target.GetInvocationList();
                foreach (var func in invocationList)
                {
                    if (func.GetType() != typeof(Action))
                    {
                        throw new Exception($"EventManager Error:{eventType} is not Action type");
                    }

                    Action action = (Action)func;
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }
            }
        }
    }

    public static class EventNameHelper
    {
        public const string OnLifeChange = "OnLifeChange";
        public const string OnScoreChange = "OnScoreChange";
        
        public const string OnDifficultyChange = "OnDifficultyChange";
        
        public const string GameOver = "GameOver";
        public const string GamePause = "GamePause";
        public const string GameReady = "GameReady";
        
        public const string StartAnswer = "StartAnswer";
        public const string EndAnswer = "EndAnswer";

        public const string AudioPause = "AudioPause";

        public const string NormalModeSwitch = "NormalModeSwitch";
        public const string AttackModeSwitch = "AttackModeSwitch";

        public const string ShowHitedNumber = "ShowHitedNumber";
        public const string ShowScoreNumber = "ShowScoreNumber";
        public const string ShowHealNumber = "ShowHealNumber";
    }
}