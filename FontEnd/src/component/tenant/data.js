export default{
	init:function(){ 
		let filter = {
			tenantName: ''
		};
		return{
			tenantData:[
            //    {
            //    	tenantNo:'2111847',
            //    	tenantName:'小明',
            //    	communityName:'乡弄里',
            //    	unit:'3栋A单元',
            //    	keeperName:'王钱'
            //    }
			],
            filter: filter,													//查询条件 
			activeCollapse: 'search',										//开关查询折叠面板
			pagination: {
				total: 0,
				current: 1,
				pageSize: 10
			},																
			editVisible: false
		}
	}
}