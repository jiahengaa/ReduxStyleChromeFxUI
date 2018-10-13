import {BaseURL, axios} from './config';

//-----------------------------------社区列表---------------------------------------------
export const getCommunities = params =>{return axios.get(`${BaseURL}/community/getCommunities`, { params: params }).then(res=>res.data)} ;

//-----------------------------------删除社区---------------------------------------------
export const deleteCommunityById = params => { return axios.get(`${BaseURL}/community/deleteCommunity`, { params: params })};


//-----------------------------------新增社区---------------------------------------------
export const addCommunity = params => { return axios.post(`${BaseURL}/community/addCommunity`,  params).then(res=>res.data)};

//-----------------------------------更新社区---------------------------------------------
export const updateCommunity = params => { return axios.post(`${BaseURL}/community/updateCommunity`,  params).then(res=>res.data)};
