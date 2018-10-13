import {BaseURL, axios} from './config';
//-----------------------------------租户列表---------------------------------------------
export const getTenantList = params => { return axios.get(`${BaseURL}/tenant/getTenantList`, { params: params }).then(res=>res.data)};

//-----------------------------------删除租户---------------------------------------------
export const removeTenantById = params => { return axios.get(`${BaseURL}/tenant/deleteTenant`, { params: params })};


//-----------------------------------新增租户---------------------------------------------
export const addTenant = params => { return axios.post(`${BaseURL}/tenant/addTenant`,  params).then(res=>res.data);};