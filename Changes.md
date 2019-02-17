# Controllers

* Base classes
    * `Controller: ApiController` -> `Controller: ControllerBase`
    * `using System.Web.Http;` -> `using Microsoft.AspNetCore.Mvc;`
* Generic return types   
    * `IHttpActionResult` -> `IActionResult` or `ActionResult<T>`
    * `Ok()` stays the same
* HTTP Attributes
    * `FromUri` -> `FromQuery`
    * `FromBody` stays the same