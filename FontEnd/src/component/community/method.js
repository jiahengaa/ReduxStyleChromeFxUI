import { getCommunities, addCommunity, updateCommunity, deleteCommunityById } from '../../api/community'
import { addTenant } from '../../api/tenant'
export default {
	//条件搜索
	handleSearch() {
		if (typeof communityActions == 'undefined') {
			this.pagination.current = 1;
			this.getTableData();
		} else {
			communityActions.handleSearch();
		}
	},
	//重置搜索条件
	resetForm(formName) {
		if (typeof communityActions == 'undefined') {
			this.$refs[formName].resetFields();
			this.getTableData();
		}else{
			communityActions.resetForm();
		}
		
	},
	//设置分页大小
	handlePageSizeChange(pageSize) {
		if (typeof communityActions == 'undefined') {
			this.pagination.pageSize = pageSize;
		this.getTableData();
		}else{
			communityActions.handlePageSizeChange(JSON.stringify(this.pagination));
		}
	},
	//设置页码
	handleCurrentChange(current) {
		if (typeof communityActions == 'undefined') {
			this.pagination.current = current;
			this.getTableData();
		}else{
			communityActions.handleCurrentChange(JSON.stringify(current))
		}
	},
	handleCloseAddDialog() {
		this.CommunityFrom = Object.assign({}, this.defaultCommunityFrom);
			this.$refs.addCommunityForm.resetFields();
			this.$refs.CreateCommunity.close();
	},
	handleCloseEditDialog() {
		this.editFrom = Object.assign({}, this.defaultCommunityFrom);
			this.$refs.editCommunityForm.resetFields();
			this.$refs.editCommunity.close();
	},
	handleSave() {                                
		this.$refs.addCommunityForm.validate((valid) => {
			if (valid) {
				if (typeof communityActions == 'undefined') {
					const params = Object.assign({}, this.CommunityFrom);
					addCommunity(params).then((res) => {
						this.$message({
							type: 'info',
							message: '新建成功'
						});
						this.$refs.CreateCommunity.close();
						this.CommunityFrom = Object.assign({}, this.defaultCommunityFrom);
						this.$refs.addCommunityForm.resetFields();
						this.getTableData();
					}).catch((err) => {
						this.$message({
							type: 'warning',
							message: '新建失败'
						});
						console.log(err);
					});
				}else{
					communityActions.addCommunity(JSON.stringify(CommunityFrom))
				}
				
			} else {
				console.log('error submit!!');
				return false;
			}
		});
	},
	editCommunity(scope) {   
		this.dialogeditCommunity = true;
			this.editFrom = Object.assign({}, {
				communityName: scope.row.communityName,
				unit: scope.row.unit,
				keeperName: scope.row.keeperName,
				message: scope.row.message,
				id: scope.row.id
			});            
		
		
	},
	updateCommunityById() {
		if (typeof communityActions == 'undefined') {
			console.log(this.communityData);
			let params = Object.assign({}, this.editFrom);
	
			console.log(params);
	
			updateCommunity(params).then((res) => {
				console.log(res);
				this.dialogeditCommunity = false;
				this.getTableData();
				this.$message.info('修改小区信息成功');
			}).catch((err) => {
				console.log(err);
				this.$message.error('修改小区信息失败');
			});
		}else{
			communityActions.updateCommunity(JSON.stringify(this.editFrom))
		}
		
	},
	delCommunity(scope) {                
		if (typeof communityActions == 'undefined') {

		}else{
			
		}        
		this.$confirm('此操作将删除选中项, 是否继续?', '提示', {
			confirmButtonText: '确定',
			cancelButtonText: '取消',
			type: 'warning'
		}).then(() => {
			this.removeCommunity(scope);
		}).catch(() => {
			this.$message({
				type: 'warning',
				message: '已取消删除'
			});
		});
	},
	removeCommunity(scope) {
		if (typeof communityActions == 'undefined') {
			const params = {
				id: scope.row.id
			};
			console.log(scope);
			deleteCommunityById(params).then((res) => {
				this.getTableData();
				this.$message({
					type: 'info',
					message: '删除成功'
				});
			}).catch((err) => {
				console.log(err);
			})
		}else{
			communityActions.deleteCommunity(JSON.stringify(scope.row.id))
		}
		
	},
	getTableData() {           
		let para = {
			pageNum: this.pagination.current,
			pageSize: this.pagination.pageSize,
			...this.filter
		};
		console.log(para);

		if (typeof communityActions == 'undefined') {
			getCommunities(para).then((res) => {
				console.log(res);
				this.communityData = res.data.records;
				this.pagination.total = res.data.total;
			});
		}else{
			communityActions.getCommunityList(JSON.stringify(this.pagination))
		}
		
		
	},
	handleCloseAddStuDialog() {
		this.tenantFrom = Object.assign({}, this.defaulttenantFrom);
		this.$refs.addStuForm.resetFields();
		this.$refs.createTenant.close();
	},
	addTenant(scope) {
		this.dialogCreateStu = true;
		this.tenantFrom.communityId = scope.row.id;
		console.log("------------------" + this.tenantFrom.communityId);
	},
	handleSaveStu() {
		this.$refs.addStuForm.validate((valid) => {
			if (valid) {
				const params = Object.assign({}, this.tenantFrom);
				addTenant(params).then((res) => {
					this.$message({
						type: 'info',
						message: '新建成功'
					});
					this.$refs.createTenant.close();
					this.tenantFrom = Object.assign({}, this.defaulttenantFrom);
					this.$refs.addStuForm.resetFields();
				}).catch((err) => {
					this.$message({
						type: 'warning',
						message: '新建失败'
					});
					console.log(err);
				});
			} else {
				console.log('error submit!!');
				return false;
			}
		});
	},
	updateData(vm, data) {
		console.log("vm:" + vm);
		console.log("data:" + data);

		for (var item in data) {
			console.log("item:" + item);
			console.log("data[" + item + "]:" + data[item])
			vm[item] = data[item];
		}
	}
}