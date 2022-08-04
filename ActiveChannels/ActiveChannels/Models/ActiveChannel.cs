using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveChannels.Models
{
    public class ActiveChannel
    {
        [StringLength(512)]
        public string Name { get; set; }
        public List<ActiveTime> ActiveTimes { get; set; }
    }
    public class ActiveTime
    {
        public ActiveTime(ActiveTime obj)
        {
            Time = obj.Time;
            Active = obj.Active;
        }
        public ActiveTime(string time, bool active)
        {
            Time = time;
            Active = active;
        }
        [StringLength(32)]
        public string Time { get; set; }
        public bool Active { get; set; }
    }
}
