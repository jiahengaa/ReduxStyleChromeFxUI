import echarts from "echarts";
export default {
    loadCommunityChartData() {
        if (typeof chartActions == 'undefined') {
            this.updateCommunityChart({
                title:'房源租住情况',
                name: '访问来源',
                data: [
                    { value: 335, name: '直接访问' },
                    { value: 310, name: '邮件营销' },
                    { value: 274, name: '联盟广告' },
                    { value: 235, name: '视频广告' },
                    { value: 400, name: '搜索引擎' }
                ]
            },null)
        } else {
            chartActions.loadCommunityChartData();
        }
    },
    updateCommunityChart(val, valOld) {
        if (!this.isObjectValueEqual(val, valOld)) {

            var curVal = {
                title:val.title,
                name:val.name,
                data:[]
            }

            for(var i = 0;i<val.data.length;i++){
                curVal.data.push({value:val.data[i].value,name:val.data[i].name})
            }

            var curChart = echarts.init(
                document.getElementById("communityResChart")
            );
            curChart.showLoading();

            var option = {
                backgroundColor: "#2c343c",

                title: {
                    text: curVal.title,
                    left: "center",
                    top: 20,
                    textStyle: {
                        color: "#ccc"
                    }
                },

                tooltip: {
                    trigger: "item",
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },

                visualMap: {
                    show: false,
                    min: 80,
                    max: 600,
                    inRange: {
                        colorLightness: [0, 1]
                    }
                },
                series: [
                    {
                        name: curVal.name,
                        type: "pie",
                        radius: "55%",
                        center: ["50%", "50%"],
                        data: curVal.data.sort(function (a, b) {
                            return a.value - b.value;
                        }),
                        roseType: "radius",
                        label: {
                            normal: {
                                textStyle: {
                                    color: "rgba(255, 255, 255, 0.3)"
                                }
                            }
                        },
                        labelLine: {
                            normal: {
                                lineStyle: {
                                    color: "rgba(255, 255, 255, 0.3)"
                                },
                                smooth: 0.2,
                                length: 10,
                                length2: 20
                            }
                        },
                        itemStyle: {
                            normal: {
                                color: "#f25d31",
                                shadowBlur: 200,
                                shadowColor: "rgba(0, 0, 0, 0.5)"
                            }
                        },

                        animationType: "scale",
                        animationEasing: "elasticOut",
                        animationDelay: function (idx) {
                            return Math.random() * 200;
                        }
                    }
                ]
            };

            curChart.setOption(option)
            curChart.hideLoading()
        }
    },
    updateData(vm, data) {
        console.log("vm:" + vm);
        console.log("data:" + data);

        for (var item in data) {
            console.log("item:" + item);
            console.log("data[" + item + "]:" + data[item])
            vm[item] = data[item];
        }
    },
    isObjectValueEqual(a, b) {

        if (!b || !a) {
            return false
        }
        var aProps = Object.getOwnPropertyNames(a);
        var bProps = Object.getOwnPropertyNames(b);


        if (aProps.length != bProps.length) {
            return false;
        }

        for (var i = 0; i < aProps.length; i++) {
            var propName = aProps[i];

            if (a[propName] !== b[propName]) {
                return false;
            }
        }
        return true;
    },
}