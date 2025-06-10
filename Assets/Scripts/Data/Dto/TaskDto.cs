using System;

namespace Data.Dto
{
    [Serializable]
    public class TaskDto
    {
        public int Id;
        public string NameId;
        public float DelayReward;
        public int BasePrice;
        public float BaseReward;
        public BoosterDto[] Boosters;
    }
}