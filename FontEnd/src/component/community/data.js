export default{
	init:function(){
			return{
				communityActions:window.communityActions,
				communityData:[],
                filter: {
					communityName: ''
				},													//查询条件 
				activeCollapse: 'search',										//开关查询折叠面板
				pagination: {
					total: 0,
					current: 1,
					pageSize: 10
				},																//巡检计划列表分页数据
				editVisible: false,
				dialogCreateCommunity: false,
				dialogeditCommunity: false,
				CommunityFrom:{
                   communityName:'',
                   unit:'',
                   keeperName:'',
                   message:''
				},
				editFrom:{
                   communityName:'',
                   unit:'',
                   keeperName:'',
                   message:'',
                   id:''
				},
				defaultCommunityFrom:{
                   communityName:'',
                   unit:'',
                   keeperName:'',
                   message:''
				},
				tenantFrom:{
					tenantName:'',
					communityId:''
				},
				defaulttenantFrom:{
					tenantName:'',
					communityId:''
				},
				dialogCreateStu:false,
				records:[],
			}
	}
}