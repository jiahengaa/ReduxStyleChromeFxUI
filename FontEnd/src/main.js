import Vue from 'vue'
import Router from 'vue-router' 
import App from './App'
import routes from './router'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-default/index.css'
import 'font-awesome/css/font-awesome.css'
import axios from 'axios'

import Mock from './mock'
import { stringify } from 'querystring';
Mock.init();

Vue.use(Router)  
Vue.use(ElementUI)
const router = new Router({
  routes
});

Vue.config.productionTip = false
Vue.prototype.$ajax =axios

Vue.config.productionTip = false
Vue.prototype.$ajax =axios

let method = {
  updateData(data){
    for (var item in data) {
      console.log('this.$root'+ this.$root)
      console.log(item);
      console.log(data[item]);
      this.$root.eventHub.$emit(item,data[item]);
    }
    
  },
}

let app = new Vue({ 
  router, 
  methods:method,
  data:{
    eventHub:new Vue()
  },
  render: h => h(App)
}).$mount('#app')

window.app = app;

export default app;