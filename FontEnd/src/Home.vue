<template>
	<el-row class="container">
		<el-col :span="24" class="header">
		    <el-col :span="20" class="logo">
				<img src="./assets/ChromFXUI.png" /> <span>社区管理<i class="txt">系统</i></span>
			</el-col>
			<el-col :span="4">
				<ul class="sys-cmd">
					<li cfx-ui-command="minimize"><i class="fa fa-window-minimize"></i></li>
					<li cfx-ui-command="maximize"><i class="fa fa-window-maximize"></i></li>
					<li cfx-ui-command="close"><i class="fa fa-window-close"></i></li>
				</ul>
			</el-col>
		</el-col>
		<el-col :span="24" class="main">
			<aside>
				 <el-menu :default-active="$route.path" class="el-menu-vertical-demo" @open="handleopen" @close="handleclose" @select="handleselect"
					theme="dark" unique-opened router>
					<template v-for="(item,index) in $router.options.routes" v-if="!item.hidden">
						<el-submenu v-bind:key="index" :index="index+''" v-if="!item.leaf">
							<template slot="title"><i :class="item.iconCls"></i>{{item.name}}</template>
							<el-menu-item v-for="child in item.children"  v-bind:key="child.index" :index="child.path" v-if="!child.hidden">{{child.name}}</el-menu-item>
						</el-submenu>
						<el-menu-item  v-bind:key="index" v-if="item.leaf&&item.children.length>0" :index="item.children[0].path "><i :class="item.iconCls"></i>{{item.children[0].name}}</el-menu-item>
					</template>
				</el-menu>
			</aside>
			<section class="content-container">
			    <div class="grid-content bg-purple-light">
					<el-col :span="24" class="breadcrumb-container">
						<strong class="title">{{$route.name}}</strong>
						<el-breadcrumb separator="/" class="breadcrumb-inner">
							<el-breadcrumb-item v-for="item in $route.matched"  v-bind:key="item.index">
								{{ item.name }}
							</el-breadcrumb-item>
						</el-breadcrumb>
					</el-col>
					<el-col :span="24" class="content-wrapper">
						<transition>
							<keep-alive>
								<router-view v-if="$route.meta.keepAlive">
									<!-- 这里是会被缓存的视图组件，比如 page1,page2 -->
								</router-view>
							</keep-alive>
							
						</transition>
						<transition>
							<router-view v-if="!$route.meta.keepAlive">
								<!-- 这里是不被缓存的视图组件，比如 page3 -->
							</router-view>
						</transition>
					</el-col>
				</div>
			</section>
		</el-col>
	</el-row>
</template>

<script>
export default {
  data() {
    return {
      sysUserName: "",
      sysUserAvatar: ""
    };
  },
  methods: {
    onSubmit() {
      console.log("submit!");
    },
    handleopen() {
      //console.log('handleopen');
    },
    handleclose() {
      //console.log('handleclose');
    },
    handleselect: function(a, b) {},
    //退出登录
    logout: function() {
      var _this = this;
      this.$confirm("确认退出吗?", "提示", {
        //type: 'warning'
      })
        .then(() => {
          sessionStorage.removeItem("user");
          _this.$router.push("/login");
        })
        .catch(() => {});
    }
  }
};
</script> 
<style scoped lang="scss">
.container {
  position: absolute;
  top: 0px;
  bottom: 0px;
  width: 100%;
  .header {
    position: absolute;
    height: 80px;
    line-height: 60px;
    background: #1f2d3d;
    color: #c0ccda;
    -webkit-app-region: drag;
    .sys-cmd {
      position: absolute;
      top: 0px;
      right: 0px;
      color: #475669;
      display: inline-block;
      vertical-align: top;
      padding: 0 10px;
      margin: 0 10px;
      cursor: pointer;
      -webkit-app-region: no-drag;
    }
    .sys-cmd li {
      color: #20a0ff;
      display: inline-flex;
      width: 32px;
      background-color: transparent;
      align-items: right;
      justify-content: center;
      transition: all ease-in-out 300ms;
      font-size: 0.8em;
    }
    .sys-cmd li:hover {
      color: violet;
    }
    .sys-cmd li:active {
      color: #c0ccda;
    }
    .logo {
      font-size: 22px;
      -webkit-app-region: drag;
      img {
        width: 40px;
        float: left;
        margin: 10px 10px 10px 18px;
        -webkit-app-region: drag;
      }
      .txt {
        color: #20a0ff;
        -webkit-app-region: drag;
      }
    }
  }
  .main {
    background: #324057;
    position: absolute;
    top: 60px;
    bottom: 0px;
    overflow: hidden;
    aside {
      width: 230px;
    }
    .content-container {
      background: #f1f2f7;
      position: absolute;
      right: 0px;
      top: 0px;
      bottom: 0px;
      left: 230px;
      overflow-y: scroll;
      padding: 20px;
      .breadcrumb-container {
        margin-bottom: 15px;
        .title {
          width: 200px;
          float: left;
          color: #475669;
        }
        .breadcrumb-inner {
          float: right;
        }
      }
      .content-wrapper {
        background-color: #fff;
        box-sizing: border-box;
      }
    }
  }
}
</style>