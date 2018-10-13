import Vue from 'vue'
import Vuex from 'vuex'
import community from './modules/community'
import tenant from './modules/tenant'

Vue.use(Vuex)

export default new Vuex.Store({
    modules:{
        community,
        tenant,
    },
})