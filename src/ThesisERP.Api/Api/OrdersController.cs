using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Application.Interfaces.Transactions;

namespace ThesisERP.Api.Api;

/// <summary>
/// Issue and manage Purchase and Sales Orders.
/// </summary>
public class OrdersController : BaseApiController
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderService _orderService;


    public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    /// <summary>
    /// Create a new Order in pending state.
    /// </summary>
    /// <remarks>
    /// Creates a new Order for the provided Supplier / Client, containing the specified product rows.
    /// </remarks>
    /// <param name="orderDTO"></param>
    /// <response code="200">Returns the created order.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericOrderDTO))]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateOrder)}");
            return BadRequest(ModelState);
        }

        var username = HttpContext.User.Identity?.Name ?? string.Empty;

        var response = await _orderService.CreateAsync(orderDTO, username);

        return Ok(response);
    }


    /// <summary>
    /// Update an order.
    /// </summary>
    /// <remarks>
    /// Only 'Pending' and 'Fulfilled' orders can be updated. <br />
    /// The Order Template cannot be updated. <br />
    /// Only orders in 'Pending' state can have their supplier/client and product rows updated. <br />    
    /// </remarks>
    /// <param name="id">The id of the order to update.</param>
    /// <param name="orderDTO">The new values of the order.</param>
    /// <response code="200">Returns the updated order.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericOrderDTO))]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO orderDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(UpdateOrder)}");
            return BadRequest(ModelState);
        }

        var response = await _orderService.UpdateAsync(id, orderDTO);

        return Ok(response);
    }

    /// <summary>
    /// Process an order, assigning it to a specified location.
    /// </summary>
    /// <remarks>
    /// Processing an order marks it ready for fulfillment and assigns it to a specific Inventory Location.     
    /// </remarks>
    /// <param name="id">The id of the order to process.</param>
    /// <param name="orderDTO">DTO containing InventoryLocationID to assign to order.</param>
    /// <response code="200">Returns the processing order.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPost("{id:int}/Process")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericOrderDTO))]
    public async Task<IActionResult> ProcessOrder(int id, [FromBody] ProcessOrderDTO orderDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(ProcessOrder)}");
            return BadRequest(ModelState);
        }

        var response = await _orderService.ProcessAsync(id, orderDTO);

        return Ok(response);
    }

    /// <summary>
    /// Fulfill an order.
    /// </summary>
    /// <param name="id">The id of the order to fulfill</param>
    /// <param name="fulfillOrderDTO">DTO with fulfillment details.</param>
    /// <remarks>
    /// Order needs to be in processing status to be fulfilled. <br />
    /// A new document will be issued to handle the fulfillment, using the provided FulfillmentDocumentTemplateId. <br /><br />
    /// To partially fulfill an order, a list of the required line numbers and associated quantities to fulfill has to be provided.
    /// </remarks>
    /// <response code="200">Returns the fulfilled order.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPost("{id:int}/Fulfill")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericOrderDTO))]
    public async Task<IActionResult> FulfillOrder(int id, [FromBody] FulfillOrderDTO fulfillOrderDTO)
    {
        if (id < 1)
        {
            return BadRequest("Order Id has to be provided.");
        }

        var response = await _orderService.FulfillAsync(id, fulfillOrderDTO);
        return Ok(response);
    }

    /// <summary>
    /// Close an order.
    /// </summary>
    /// <remarks>
    /// Closing an order means it is considered completed, and no further updates can take place.
    /// </remarks>
    /// <param name="id">The id of the order to close</param>
    /// <response code="200">Returns the closed order.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPost("{id:int}/Close")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericOrderDTO))]
    public async Task<IActionResult> CloseOrder(int id)
    {
        if (id < 1)
        {
            return BadRequest("Order Id has to be provided.");
        }

        var response = await _orderService.CloseAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Cancel an order.
    /// </summary>
    /// <remarks>
    /// An order can be cancelled only if all open related documents are cancelled first.
    /// </remarks>
    /// <param name="id">The id of the order to cancel</param>
    /// <response code="200">Returns the cancelled order.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPost("{id:int}/Cancel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericOrderDTO))]
    public async Task<IActionResult> CancelOrder(int id)
    {
        if (id < 1)
        {
            return BadRequest("Order Id has to be provided.");
        }

        var response = await _orderService.CancelAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Retrieve all orders.
    /// </summary>
    /// <response code="200">A list of all orders in your account.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GenericOrderDTO>))]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderService.GetOrdersAsync();

        return Ok(orders);
    }

    /// <summary>
    /// Retrieve an order by id
    /// </summary>
    /// <param name="id">The id of the order to retrieve</param>
    /// <response code="200">Returns the requested order.</response>
    /// <response code="404">If order does not exist.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericOrderDTO))]
    public async Task<IActionResult> GetOrder(int id)
    {
        if (id < 1)
        {
            return BadRequest("Order Id has to be provided.");
        }

        var order = await _orderService.GetOrderAsync(id);

        if (order == null) { return NotFound(); }

        return Ok(order);
    }
}
