using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Packages
{
    public struct TenantState
    {
        public TenantFilter filter;
        public bool editVisible;
        public Pagination pagination;
        public string activeCollapse;
        public List<Tenant> tenantData;
        public List<Tenant> records;
        public int total;
    }
}
