using Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityActions
{
    public class handleSearch { }
    public class resetForm { }

    public class handlePageSizeChange { public int pageSize; }
    public class handleCurrentChange { public int current; }
    public class addCommunity { public CommunityFrom communityFrom;}

    public class updateCommunity { public EditFrom editFrom; }

    public class deleteCommunity { public int id; }

    public class getCommunityList { public Pagination pagination; }

    public class loadCommunityList { }
}
