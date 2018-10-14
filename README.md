# ReduxStyleChromeFxUI
    Do Something Funny!

  更高效，更简洁，更现代化PC端桌面开发。
  采用redux设计模式，采用chrome为UI内核渲染，采用ElementUI为前端渲染，采用Vue为前端双向绑定，采用前端“热加载”快速开发。

## 背景问题
    在C#.net传统PC开发中还在苦恼缓慢，界面UI老旧不易美化的问题吗？
  常常为了**Model**,**View**,**Controller**层分离而苦恼，代码结构混乱而烦恼？
  常常为了数据绑定问题使用MVVM而需要引用第三方组件库如mvvmlight或者DevExpress等控件库提供的绑定方案吗？
  常常为了界面库需要动画或者特殊透明效果等需求而受限于控件库，不得已去操作GDI去绘制吗？常常苦于单元化测试困难吗，
  担心API代码质量吗？此框架的诞生就是为了解决以上问题。
## 解决方案灵感
    目前web前端开发界面开发繁荣发展，比传统桌面端开发效率高、颜值高、稳定且功能丰富。
如果能将web前端作为传统UI设计的替代就非常完美了，实践证明也确实非常好。借助chrome的cef，能很好的解决此问题。但是这样会引入js代码的问题思考。如果将传统PC端APP开发中的Controller控制模块用js实现，那肯定是个灾难性的场面。必定会引发问题定位麻烦，代码不好维护管理等问题。**Redux**简单而纯粹的架构思想能完美解决此问题。在此项目框架中，web端所有的页面及控件所使用的js,html,css代码仅定位于实现控件逻辑及UI效果，通过数据绑定驱动其更新。web端的数据管理问题是个头疼的事情，在这里使用Redux和vue的有机搭配完美处理了此问题。所有的web均使用vue提供的**MVVM**功能，UI更新由Redux的State更新而驱动。Controller模块使用强类型的C#语言实现，且易于调试测试。额外优势是可以动态从静态资源服务器获取资源更新内容，这算是利用了h5的 一大特点吧。
## _混合框架架构_
    这是一个基于CEF的桌面混合架构，有机结合多种技术的优势，解决PC开发中一直困扰的诸多问题。
![混合架构图](https://github.com/jiahengaa/ReduxStyleChromeFxUI/blob/master/DemoShot/architecture%20.png)
## Demo shot
![example1.png](https://github.com/jiahengaa/ReduxStyleChromeFxUI/blob/master/DemoShot/example1.png)
## 负面效应
    当然所有的框架都会有其缺点，此框架也不例外。
    1.所使用技术内容涵盖非常广而杂，给研发人员带来挑战；
    2.纯web前端开发人员得学会使用C#语言，或者c#语言的非web开发人员得了解前端开发，混合型人才就不担心这个问题啦；
    3.web端的调试问题，但是在框架中使用**mock**来绕道解决此问题，使得web开发不绝对依赖C#端的Controller的Api的完成；
    4.应用程序的打包大小等于chrome cef的内核大小 + 引用项目dll + web资源包大小，基本上在230M以上。
    但是引用了DevExpress等架构库，带来的麻烦不必这个小，而且会有很多dll依赖、性能、发布问题。

