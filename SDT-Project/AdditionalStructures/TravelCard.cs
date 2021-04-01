using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDT_Project.AdditionalStructures
{
    public enum CardCategories { A = 1, T, M, Tram, AT, ATM, ATTram, ATMTram };
    public enum CardStatuses { Active = 1, Inactive };
    public enum CardTypes { Base = 1, Violet, Red, Full}
    public class TravelCard
    {
        public int Id { set; get; }
        public CardTypes Type { set; get; }
        public int Balance { set; get; }
        public DateTime FirstDate { set; get; }
        public DateTime LastDate { set; get; }
        public CardCategories Category { set; get; }
        public CardStatuses Status { set; get; }
    }
}
