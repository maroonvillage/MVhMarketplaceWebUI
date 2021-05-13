using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webui.Models
{
    public class HomeModel : DefaultModel
    {
        private readonly IServiceProvider _serviceProvider1;
        public int BlockId { get; set; }

        public HomeModel()
        {

        }
        public HomeModel(IServiceProvider serviceProvider)
        {
            _serviceProvider1 = serviceProvider;
        }

        public bool HasServiceProvider()
        {
            return (_serviceProvider1 != null);
        }


    }
}
