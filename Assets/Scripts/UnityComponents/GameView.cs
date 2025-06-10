using System.Collections.Generic;
using Components;
using Data.SO;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;

namespace UnityComponents
{
    public class GameView : MonoBehaviour
    {
        [Header("Currency text")]
        [SerializeField]
        private TMP_Text currencyText;
        
        [Header("Tasks Container")]
        [SerializeField]
        private RectTransform tasksContainer;
        
        [Header("Task View")]
        [SerializeField]
        private TaskView taskView;
        
        private Localized _localized;

        public void Init(Localized localized)
        {
            _localized = localized;
        }

        public TaskView CreateTask(EcsEntity entity)
        {
            var taskObjectView = Instantiate(taskView, tasksContainer, false);
            taskObjectView.Init(entity);
            return taskObjectView;
        }

        public void UpdateCurrency(float value)
        {
            var temp = _localized.GetLocalized("balance");
            currencyText.text = temp.Replace("{0}", value.ToString());
        }
    }
}
