using System.Collections.Generic;
using System.Linq;
using Data.Dto;
using UnityEngine;

namespace Data.SO
{
    [CreateAssetMenu(fileName = "Localized", menuName = "Game/Localized", order = 51)]
    public class Localized : ScriptableObject
    {
        public List<LocalizedStringDto> texts = new List<LocalizedStringDto>();

        public string GetLocalized(string id)
        {
            var value = texts.FirstOrDefault(x => x.Id == id)?.Value;
            if (value != null)
            {
                return value;
            }
            Debug.Log("Localized Not found localized id: " + id);
            return "Not found localized id: " + id;
        } 
    }
}
