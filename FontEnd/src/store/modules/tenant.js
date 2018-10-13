const state = {
    filter:{tenantName:''},
    editVisible:true,
    pagination:{
        pageSize:10,
        current:1,
        total:0,
    },
    activeCollapse:"search",
    tenantData:[],
    records:[],
    total:0,
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