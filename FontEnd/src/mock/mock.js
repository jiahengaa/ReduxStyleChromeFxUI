import axios from 'axios';
import MockAdapter from 'axios-mock-adapter'
import Mock from 'mockjs'
import { Communities, Tenants } from './model/models'

let _communities = Communities;
let _tenants = Tenants;

export default {
    init() {
        let mock = new MockAdapter(axios);
        mock.onGet('/community/getCommunities').reply(config => {
            let { communityName } = config.params;
            let mockCommunities = _communities.filter(c => {
                if (communityName && c.communityName.indexOf(communityName) == -1) {
                    return false;
                }
                else {
                    return true;
                }
            });
            return new Promise((resolve, reject) => {
                setTimeout(() => {
                    resolve(
                        [
                            200,
                            {
                                data: {
                                    records: mockCommunities,
                                    total: mockCommunities.length
                                }
                            }
                        ]
                    )
                }, 1000);
            });
        });

        mock.onGet('/tenant/getTenantList').reply(config => {
            let { tenantName,pageSize,pageNum } = config.params;
            let mockTenants = _tenants.filter(tenant => {
                if (tenantName && tenant.tenantName.indexOf(tenantName) == -1) {
                    return false;
                }
                else {
                    return true;
                }
            });
            let curPage = mockTenants.filter((u, index) => index < pageSize * pageNum && index >= pageSize * (pageNum - 1));
            return new Promise((resolve, reject) => {
                setTimeout(() => {
                    resolve([
                        200,
                        {
                            data: {
                                records: curPage,
                                total: mockTenants.length
                            }
                        }
                    ]
                    );
                }, 1000);
            });
        });

        mock.onGet('/community/deleteCommunity').reply(config => {
            let { id } = config.params;
            _communities = _communities.filter(c => c.id !== id);
            return new Promise((resolve, reject) => {
                setTimeout(() => {
                    resolve([200, {
                        code: 200,
                        msg: '删除成功了！'
                    }])
                }, 500);
            });
        });

        mock.onPost('/community/addCommunity').reply(config => {
            let {communityName, unit,keeperName,message} = JSON.parse(config.data);
            _communities.push({
                communityNo: Mock.Random.integer(),
                communityName: communityName,
                message: message,
                unit: unit,
                id: Mock.Random.integer(),
                keeperName: keeperName
            });
            return new Promise((resolve, reject) => {
                setTimeout(() => {
                    resolve([200, {
                        code: 200,
                        msg: '新增成功了！'
                    }])
                }, 500);
            });
        });

        mock.onPost('/community/updateCommunity').reply(config => {
            let { id, communityName, unit, keeperName, message } =JSON.parse(config.data);
            _communities.some(c => {
                if (c.id === id) {
                    c.communityName = communityName,
                        c.unit = unit,
                        c.keeperName = keeperName,
                        c.message = message
                }
            });
            return new Promise((resolve, reject) => {
                setTimeout(() => {
                    resolve([200, {
                        code: 200,
                        msg: '编辑成功'
                    }])
                }, 1000);
            });
        });

        mock.onPost('/tenant/addTenant').reply(config => {
            let { tenantName} = JSON.parse(config.data);
            _tenants.push({
                communityName: Mock.Random.name(false),
                tenantNo: Mock.Random.id(),
                unit: 2,
                keeperName: Mock.Random.cname(),
                tenantName: tenantName,
                message:Mock.Random.string(),
                id: Mock.Random.guid()
            });
            return new Promise((resolve, reject) => {
                setTimeout(() => {
                    resolve([200, {
                        code: 200,
                        msg: '新增成功了！'
                    }])
                }, 500);
            });
        });

        mock.onGet('/tenant/deleteTenant').reply(config => {
            let { id } = config.params;
            _tenants = _tenants.filter(s => s.id !== id);
            return new Promise((resolve, reject) => {
                setTimeout(() => {
                    resolve([200, {
                        code: 200,
                        msg: '删除成功了！'
                    }])
                }, 500);
            });
        });
    }
};