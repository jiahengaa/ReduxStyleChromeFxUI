using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Packages
{
    public struct CommunityState
    {
        public Pagination pagination;
        public List<Community> communityData;
        public string activeCollapse;//search
        public CommunityFilter filter;
        public bool editVisible;
        public bool dialogCreateCommunity;
        public bool dialogeditCommunity;
        public CommunityFrom CommunityFrom;
        public EditFrom editFrom;
        public DefaultCommunityFrom defaultCommunityFrom;
        public TenantFrom tenantFrom;
        public TenantFrom defaulttenantFrom;
        public bool dialogCreateStu;
        public List<Community> records;
    }
}
