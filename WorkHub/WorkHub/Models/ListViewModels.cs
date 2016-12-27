using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkHub.Models
{
    public class MainListViewModel
    {
        public List<WorkOrder> WorkOrders;
    }

    public class NearbyWorkOrdersViewModel
    {
        public List<WorkOrder> WorkOrders;
        public int Range;
    }
}