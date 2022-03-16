using PizzaWebsite.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    public class OwnerViewModel
    {
        public List<FullUserInfo> EmployeeInfos { get; set; }
    }
}
