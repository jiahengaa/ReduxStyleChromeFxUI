using Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TenantActions
{
    public class handleSearch { public int current; }
    public class resetForm { }
    public class handlePageSizeChange { public Pagination pagination; }

    public class handleCurrentChange { public int current; }

    public class removeTenantById { public int id; }

    public class getTenantList { public int pageNum;public int pageSize;public string tenantName; }
}