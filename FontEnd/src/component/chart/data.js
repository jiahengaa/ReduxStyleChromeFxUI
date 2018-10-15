export default {
    init: function () {
        return {
            communityData: {
                title:'房源租住情况',
                name: '访问来源',
                data: [
                    { value: 335, name: '直接访问' },
                    { value: 310, name: '邮件营销' },
                    { value: 274, name: '联盟广告' },
                    { value: 235, name: '视频广告' },
                    { value: 400, name: '搜索引擎' }
                ]
            },
            tenantData: {
                name: '租户租住时长',
                data: []
            },
        };
    }
}