using JobBoardPlatform.Application.Common.Dto.RequestDto.PaymentDto;
using JobBoardPlatform.Application.Interfaces.PaymentInterface;
using JobBoardPlatform.Mvc.Models.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.Mvc.Controllers;

[Authorize(Policy = "ApprovedEmployerOnly")]
public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public IActionResult Feature(Guid advertisementId)
    {
        var options = _paymentService.GetFeaturedOptions();

        return View(FeaturedPaymentViewModel.FromResponseDto(advertisementId, options));
    }

    [HttpPost]
    public async Task<IActionResult> Feature(FeaturedPaymentViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            model.Options = _paymentService.GetFeaturedOptions();
            return View(model);
        }

        var paymentId = await _paymentService.CreateFeaturedPaymentAsync(
            new CreateFeaturedPaymentRequestDto
            {
                AdvertisementId = model.AdvertisementId,
                DurationInDays = model.DurationInDays
            },
            cancellationToken);

        return RedirectToAction(nameof(Pay), new { id = paymentId });
    }

    [HttpGet]
    public async Task<IActionResult> Pay(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetPaymentAsync(id, cancellationToken);

        return View(PaymentProcessingViewModel.FromResponseDto(payment));
    }

    [HttpGet]
    public async Task<IActionResult> Success(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetPaymentAsync(id, cancellationToken);

        await _paymentService.ConfirmSuccessfulPaymentAsync(id, cancellationToken);

        TempData["Success"] = "Payment was successful. Your advertisement is now featured.";

        return RedirectToAction("Details", "Advertisement", new { id = payment.AdvertisementId });
    }

    [HttpGet]
    public async Task<IActionResult> Fail(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetPaymentAsync(id, cancellationToken);

        await _paymentService.ConfirmFailedPaymentAsync(id, cancellationToken);

        TempData["Error"] = "The payment failed. Please try again.";

        return RedirectToAction("Details", "Advertisement", new { id = payment.AdvertisementId });
    }

    [HttpGet]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetPaymentAsync(id, cancellationToken);

        await _paymentService.CancelPaymentAsync(id, cancellationToken);

        TempData["Error"] = "The payment was cancelled.";

        return RedirectToAction("Details", "Advertisement", new { id = payment.AdvertisementId });
    }
}
