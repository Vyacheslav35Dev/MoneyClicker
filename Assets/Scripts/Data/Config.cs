using System.Collections.Generic;
using Data.Dto;
using UnityEngine;

namespace Data.SO
{
    [CreateAssetMenu(fileName = "Config", menuName = "Game/Config", order = 51)]
    public class Config : ScriptableObject
    {
        public List<TaskDto> Tasks;
    }
}
