const state = {
    pagination: {
        total: 0,
        current: 1,
        pageSize: 10
    },
    communityData:[],
    activeCollapse:'search',
    filter: {
        communityName: ''
    },	
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
				dialogCreateStu:false
}

const getters={

}

const actions ={

}

const mutations = {

}

export default{
    namespaced:true,
    state,
    getters,
    actions,
    mutations
}