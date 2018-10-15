<template>
    <div style="padding:20px">
        <el-row>
            <el-col :span="12">
                <div id="communityResChart" style="height:300px"></div>
            </el-col>
            <el-col :span="12">
                <div id="tenantChart" style="height:300px"></div>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12"></el-col>
        </el-row>
        
    </div>
</template>

<script  type="text/javascript">
import data from "./data";
import methods from "./method"
export default {
  data() {
    return data.init();
  },
  mounted: function(){
	  	  this.$nextTick(function(){
		  this.$root.eventHub.$on('chart',data=>{
			  console.log('chart this.$data:'+this.$data)
			  console.log('chart data:'+data)
			  console.log("methods:"+methods)
			  methods.updateData(this.$data,data)
          })
          
        methods.loadCommunityChartData()
	  })
  },
 watch: {
    communityData: {
        handler(val,oldVal){
             console.log('new: %s, old: %s', val, oldVal)
            methods.updateCommunityChart(val,oldVal);
        },
        deep:true,
    }
  }
};
</script>
