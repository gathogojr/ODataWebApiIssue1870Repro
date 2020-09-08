<!-- markdownlint-disable MD002 MD041 -->
Repro for OData Web Api issue [#1870](https://github.com/OData/odata.net/issues/1870)

Based on OData spec, section [4.11 Addressing Derived Types](http://docs.oasis-open.org/odata/odata-data-aggregation-ext/v4.0/cs02/odata-data-aggregation-ext-v4.0-cs02.html#_Toc435016582), here's what is expected:

_Example 36: entity set restricted to VipCustomer instances_
```
http://host/service/Customers/Model.VipCustomer
```
_Example 37: entity restricted to a VipCustomer instance, resulting in 404 Not Found if the customer with key 1 is not a VipCustomer_
```
http://host/service/Customers/Model.VipCustomer(1)

http://host/service/Customers(1)/Model.VipCustomer
```
_Example 39: filter expression with type cast; will evaluate to null for all non-VipCustomer instances and thus return only instances of VipCustomer_
```
http://host/service/Customers?$filter=Model.VipCustomer/PercentageOfVipPromotionProductsOrdered gt 80
```
_Example 40: expand the single related Customer only if it is an instance of Model.VipCustomer. For to-many relationships only Model.VipCustomer instances would be inlined,_
```
http://host/service/Orders?$expand=Customer/Model.VipCustomer
```

Based on my repro only scenario in Example 39 appeared to work for me.
```
http://localhost:32522/odata/Customers?$filter=NS.Models.VipCustomer/LoyaltyCardNo eq '9876543210'
```

The following appear not to work:

### Scenario in Example 36:
```
http://localhost:32522/odata/Customers/NS.Models.VipCustomer
```
Responds with a 404 yet there's 1 `VipCustomer` instance

### Scenario in Example 37:
```
http://localhost:32522/odata/Customers/Model.VipCustomer(2)
```
Responds with a 404 yet customer with `Id` 2 is a `VipCustomer`
```
http://localhost:32522/odata/Customers(1)/NS.Models.VipCustomer
```
Returns a response yet customer with `Id` 1 is not a `VipCustomer`. It should respond with a 404

### Scenario in Example 40:
```
http://localhost:32522/odata/Orders?$expand=Customer/NS.Models.Customer
```
Returns an error:
```The query specified in the URI is not valid. Found a path traversing multiple navigation properties. Please rephrase the query such that each expand path contains only type segments and navigation properties.```
Inner error:
```
Found a path traversing multiple navigation properties. Please rephrase the query such that each expand path contains only type segments and navigation properties.
```

The issue the customer reported is based on this last scenario