using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace SDT_Project_Host
{
    class Host
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(SDT_Project_Service.ServiceServer)))
            {
                host.Open();
                Console.WriteLine("Хост стартовал!");
                Console.ReadLine();
            }
        }
    }
}
