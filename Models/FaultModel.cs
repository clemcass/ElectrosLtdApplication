using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using Common;

namespace ElectrosLtdApplication.Models
{
    public class FaultModel
    {
        public int FaultId { get; set; }
        public int TicketNo { get; set; }
        public Byte[] Barcode { get; set; }
        public string Details { get; set; }
        public int OrderItemId  { get; set; }


        public virtual OrderItem OrderItem { get; set; }
        public virtual ICollection<FaultTrack> FaultTrack { get; set; }
        public virtual FaultTrack LastFaultTrack { get; set; }
        public List<FaultTrack> FaultTrackList { get; set; }
    }
}