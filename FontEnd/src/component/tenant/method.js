import { getTenantList, removeTenantById } from '../../api/tenant'
export default {
	//条件搜索
	handleSearch() {
		this.pagination.current = 1;
		this.getTableData()
	},
	//重置搜索条件
	resetForm(formName) {
		this.$refs[formName].resetFields();
	},
	//设置分页大小
	handlePageSizeChange(pageSize) {
		this.pagination.pageSize = pageSize;
		this.getTableData();
	},
	//设置页码
	handleCurrentChange(current) {
		this.pagination.current = current;
		this.getTableData();
	},
	delTenant(scope) {                            //---------------------删除租户
		this.$confirm('此操作将删除选中项, 是否继续?', '提示', {
			confirmButtonText: '确定',
			cancelButtonText: '取消',
			type: 'warning'
		}).then(() => {
			this.removeTenant(scope);
		}).catch(() => {
			this.$message({
				type: 'warning',
				message: '已取消删除'
			});
		});
	},
	removeTenant(scope) {
		const params = {
			id: scope.row.id
		};
		console.log(scope);
		removeTenantById(params).then((res) => {
			this.getTableData();
			this.$message({
				type: 'info',
				message: '删除成功'
			});
		}).catch((err) => {
			console.log(err);
		})
	},
	getTableData() {                           //---------------------获取列表数据
		let para = {
			pageNum: this.pagination.current,
			pageSize: this.pagination.pageSize,
			...this.filter
		};
		console.log(para);
		getTenantList(para).then((res) => {
			console.log(res);
			this.tenantData = res.data.records;
			this.pagination.total = res.data.total;
		});
	},
	updateData(vm, data) {
		for (var item in data) {
			console.log("item:" + item);
			console.log("data[" + item + "]:" + data[item])
			vm[item] = data[item];
		}
	}
}