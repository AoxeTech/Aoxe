# QueryService

---

## 说明

* 在微服务级别的CQS中Q端的实现。
* 当前QueryService为一个类库，实际上依据团队规模和开发成本，QueryService可以作为一个单独的微服务而部署。
* CQS中，Q端并不属于OO的范畴，因此使用恰当（技术、成本、资源）的实现即可。
* CQRS中，Q端的读模型实际上也相当于聚合的一种，其提供的接口也可使用**聚合即资源**的restful风格。
* 当前示例使用了ADO.Net，实际操作中可使用ef的dbfirst模式（ef core已取消）、micro orm（如dapper、freesql等）或者一些lambda to sql的封装来实现。