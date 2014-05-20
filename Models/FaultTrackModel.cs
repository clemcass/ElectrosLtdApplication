using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace ElectrosLtdApplication.Models
{
    public class FaultTrackModel
    {
        public int FaultTrackId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int FaultId { get; set; }

        public virtual Fault Fault { get; set; }
        public List<FaultTrack> FaultTrackList { get; set; }


    }
}