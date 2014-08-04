using System;
using System.Collections.Generic;
using System.Text;

namespace BWQS_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var BWQSInstance = new BWQS.BWQS(); 
           
            var predicateSelection = BWQSInstance.Query("22", "j");
            var filterCriteria = BWQSInstance.Where("2::Renato", "j");
            var itemsClassification = BWQSInstance.OrderBy("2", "j");
        }
    }
}
