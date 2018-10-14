<template>
	<div style="padding: 20px;"> 
        <!-- -------------------------------------------查询条件--------------------------------------------- -->			
		<el-collapse v-model='activeCollapse'>
			<el-collapse-item title='查询条件' name='search'>
				<el-form ref='searchCondition' :model='filter' label-width='80px' label-position=‘left’>
					<el-row>
						<el-col :span='6'>
							<el-form-item label='社区名称' prop='communityName'>
								<el-input v-model='filter.communityName' :clearable='true'></el-input>
							</el-form-item>
						</el-col> 
						<el-col :span='6' style="marginLeft:20px;">
                            <el-button @click='handleSearch' type='primary'>查询</el-button>
					        <el-button @click='resetForm("searchCondition")'>重置</el-button>
						</el-col>
					</el-row> 
				</el-form> 
			</el-collapse-item>
		</el-collapse> 
		<!-- -------------------------------------------社区列表--------------------------------------------- -->
			<div>
				<el-row type='flex' justify='end'  style='margin: 20px 0 10px 0'>
					<el-button @click='dialogCreateCommunity=true' type='primary'>新建社区</el-button>
					<!-- <el-button @click='handleCancelPlan' type='danger'>删除巡检计划</el-button> -->
				</el-row>
				
				<div style='margin-bottom: 20px'>
					<el-table :data='records' width='100%' border>
						<el-table-column type='index' width='65'></el-table-column>
						<el-table-column label='社区编号' prop='communityNo' width='200'></el-table-column>
						<el-table-column label='社区名称' prop='communityName' width='200'></el-table-column>
						<el-table-column label='所属单元' prop='unit' width='200'></el-table-column>  
						<el-table-column label='物业管理员' prop='keeperName' width='200'></el-table-column> 
						<el-table-column label='社区描述' prop='message'></el-table-column> 
						<el-table-column label='操作' prop='operate'>
							<template scope="scope">
						        <el-button @click="editCommunity(scope)" type="button" size="primary">编辑</el-button>
						        <el-button @click="delCommunity(scope)" type="danger" size="primary">删除</el-button> 
						        <el-button @click="addTenant(scope)" type="success" size="primary">添加租户</el-button> 
						    </template>
						</el-table-column> 
					</el-table>
				</div>
				
				<el-row type='flex' justify='end'>
					<el-pagination
						@size-change='handlePageSizeChange'
						@current-change='handleCurrentChange'
						:current-page='pagination.current'
						:page-sizes='[10, 20, 50, 100]'
						:page-size='pagination.pageSize'
						:total='pagination.total'
						layout='prev,pager,next,jumper,total,sizes'
					></el-pagination>
				</el-row>
			</div> 
			<!-- -------------------------------------------------新建社区----------------------------------------------- -->
			<el-dialog title='新建社区' size='large' ref='CreateCommunity' v-model='dialogCreateCommunity' style="width:800px;padding: 50px;marginLeft:400px;">
				<div style='padding: 20px 50px'>
					<el-form label-width='100px' ref='addCommunityForm' :model='CommunityFrom'>
						<el-row type='flex' justify='space-between'>
							<el-col :span='11'>
								<el-form-item
									label='社区名称'
									prop='communityName'
									required
									:rules='{
								      	required: true, message: "名称不能为空！"
								    }'
								>
									<el-input v-model='CommunityFrom.communityName' :maxlength='50'></el-input>
								</el-form-item>
							</el-col>
							<el-col  :span='11'>
								 <el-form-item
									label='所属单元'
									prop='unit'
									required
									:rules='{
								      	required: true, message: "单元不能为空！"
								    }'
								>
									<el-input v-model='CommunityFrom.unit' :maxlength='50'></el-input>
								</el-form-item>
							</el-col>
						</el-row> 
						<el-row type='flex' justify='space-between'>
							<el-col :span='11'>
								<el-form-item
									label='物业管理员'
									prop='keeperName'
									required
									:rules='{
								      	required: true, message: "物业管理员不能为空！"
								    }'
								>
									<el-input v-model='CommunityFrom.keeperName' :maxlength='50'></el-input>
								</el-form-item>
							</el-col>
							<el-col  :span='11'>
								 <el-form-item
									label='社区描述'
									prop='message'
									required
									:rules='{
								      	required: true, message: "社区描述不能为空！"
								    }'
								>
									<el-input v-model='CommunityFrom.message' :maxlength='128'></el-input>
								</el-form-item>
							</el-col>
						</el-row> 
					</el-form>
					<el-row type='flex' justify='end'>
						<el-button @click='handleCloseAddDialog'>取消</el-button>
						<el-button type='info' @click='handleSave'>保存</el-button>
					</el-row>
				</div>
			</el-dialog>

			<!-- -------------------------------------------------添加租户----------------------------------------------- -->
			<el-dialog title='添加租户' size='large' ref='createTenant' v-model='dialogCreateStu' style="width:800px;padding: 50px;marginLeft:400px;">
				<div style='padding: 20px 50px'>
					<el-form label-width='100px' ref='addStuForm' :model='tenantFrom'>
						<el-row type='flex' justify='space-between'>
							<el-col :span='11'>
								<el-form-item
									label='租户姓名'
									prop='tenantName'
									required
									:rules='{
								      	required: true, message: "名称不能为空！"
								    }'
								>
									<el-input v-model='tenantFrom.tenantName' :maxlength='50'></el-input>
								</el-form-item>
							</el-col> 
						</el-row>
					</el-form>
					<el-row type='flex' justify='end'>
						<el-button @click='handleCloseAddStuDialog'>取消</el-button>
						<el-button type='info' @click='handleSaveStu'>保存</el-button>
					</el-row>
				</div>
			</el-dialog>

			<!-- -------------------------------------------------编辑社区----------------------------------------------- -->
			<el-dialog title='新建社区' size='large' ref='editCommunity' v-model='dialogeditCommunity' style="width:800px;padding: 50px;marginLeft:400px;">
				<div style='padding: 20px 50px'>
					<el-form label-width='100px' ref='editCommunityForm' :model='editFrom'>
						<el-row type='flex' justify='space-between'>
							<el-col :span='11'>
								<el-form-item
									label='社区名称'
									prop='communityName'
									required
									:rules='{
								      	required: true, message: "名称不能为空！"
								    }'
								>
									<el-input v-model='editFrom.communityName' :maxlength='50'></el-input>
								</el-form-item>
							</el-col>
							<el-col  :span='11'>
								 <el-form-item
									label='所属单元'
									prop='unit'
									required
									:rules='{
								      	required: true, message: "单元不能为空！"
								    }'
								>
									<el-input v-model='editFrom.unit' :maxlength='50'></el-input>
								</el-form-item>
							</el-col>
						</el-row> 
						<el-row type='flex' justify='space-between'>
							<el-col :span='11'>
								<el-form-item
									label='物业管理员'
									prop='keeperName'
									required
									:rules='{
								      	required: true, message: "物业管理员不能为空！"
								    }'
								>
									<el-input v-model='editFrom.keeperName' :maxlength='50'></el-input>
								</el-form-item>
							</el-col>
							<el-col  :span='11'>
								 <el-form-item
									label='社区描述'
									prop='message'
									required
									:rules='{
								      	required: true, message: "社区描述不能为空！"
								    }'
								>
									<el-input v-model='editFrom.message' :maxlength='128'></el-input>
								</el-form-item>
							</el-col>
						</el-row> 
					</el-form>
					<el-row type='flex' justify='end'>
						<el-button @click='handleCloseEditDialog'>取消</el-button>
						<el-button type='info' @click='updateCommunityById'>保存</el-button>
					</el-row>
				</div>
			</el-dialog>
	</div>
</template>
<script type="text/javascript">
import methods from "./method";
import data from "./data";

export default  {
  data() {
    return data.init();
  },
  methods: methods,
  mounted: function(){
	  	  this.$nextTick(function(){
		  this.$root.eventHub.$on('community',data=>{
			  console.log('community this.$data:'+this.$data)
			  console.log('community data:'+data)
			  console.log("methods:"+methods)
			  methods.updateData(this.$data,data)
		  })
	  })
  },
  created() {
    this.getTableData();
  }
};

</script>
<style type="text/css">
</style>