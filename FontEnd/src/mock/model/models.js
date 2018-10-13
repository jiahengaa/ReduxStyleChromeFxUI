import Mock from 'mockjs';
const Communities = [
    {
        id: Mock.Random.integer(),
        communityNo: Mock.Random.id(),
        communityName: Mock.Random.name(false),
        unit: 2,
        keeperName: Mock.Random.cname(),
        message: Mock.Random.string(),
    },
    {
        id: Mock.Random.integer(),
        communityNo: Mock.Random.id(),
        communityName: Mock.Random.name(false),
        unit: 2,
        keeperName: Mock.Random.cname(),
        message: Mock.Random.string(),
    }, {
        id: Mock.Random.integer(),
        communityNo: Mock.Random.id(),
        communityName: Mock.Random.name(false),
        unit: 2,
        keeperName: Mock.Random.cname(),
        message: Mock.Random.string(),
    },
];

const Tenants = [];
for (let i = 0; i < 100; i++) {
    Tenants.push(
        Mock.mock({
            id: Mock.Random.guid(),
            tenantNo: Mock.Random.id(),
            communityName: Mock.Random.name(false),
            unit: 2,
            tenantName: Mock.Random.cname(),
            keeperName: Mock.Random.cname(),
            message: Mock.Random.string(),
        })
    )
}

export { Communities, Tenants }