<template>
  <div id="app">
    <img src="./assets/ChromFXUI.png">
    <router-view></router-view>
  </div>
</template> 
<script>
export default { name: 'app'}
</script> 
<style>
body {
    margin: 0px;
    padding: 0px; 
    background: #1F2D3D;
    font-family: Helvetica Neue, Helvetica, PingFang SC, Hiragino Sans GB, Microsoft YaHei, SimSun, sans-serif;
    font-size: 14px;
    -webkit-font-smoothing: antialiased;
  } 
  #app {
    position: absolute;
    top: 0px;
    bottom: 0px;
    width: 100%;
    min-width: 100px;
  } 
  .el-submenu [class^=fa] {
    vertical-align: baseline;
    margin-right: 10px;
  } 
  .el-menu-item [class^=fa] {
    vertical-align: baseline;
    margin-right: 10px;
  } 
  .toolbar {
    background: #fff;
    padding: 10px;
    
  }
  .el-form-item {
      margin-bottom: 10px;
    }
</style>
