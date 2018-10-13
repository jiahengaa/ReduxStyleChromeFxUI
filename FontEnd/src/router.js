
import Home from './Home' 
import community from './component/community/community'
import tenant from './component/tenant/tenant'

let router = [
   {
      path: '/',
      name: '社区',
      component: Home,
      redirect: '/community',
	  iconCls: 'fa fa-home',
	  children: [
    		{ path: '/community', component: community, name: '社区管理',meta:{keepAlive: true} },
    		{ path: '/tenant', component: tenant, name: '租户管理',meta:{keepAlive: false} }
      ] 
    }
]; 
export default router;
