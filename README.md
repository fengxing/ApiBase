# ApiBase---webapi顶层设计层

项目其他描述:</br>
http://chenzhuojie.com/project/redefine%20webapi.html</br>
http://chenzhuojie.com/project/build%20apibase.html</br>

##主要功能介绍

控制器</br>
1.ErrorController:默认定义错误路由,做标准化的返回</br>
2.FindController：发现服务,后续还需要大幅度的重构，暂时功能可用。</br>
3.HeartController:心跳服务。</br></br>

功能</br>
1.一段时间内的重复请求只访问一次业务代码[可配置]</br>
2.统一异常处理</br>
3.统一结果处理.对象转json.空对象、值类型返回文本</br>
4.开发环境快速调试特性HttpGetDelegatingHandler</br>
5.统一状态码返回.</br>


