using DessertERP.Application.Modules.AccountsPayable.DTOs;
using DessertERP.Application.Modules.AccountsPayable.Services;
using DessertERP.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[Authorize(Policy = PermissionKeys.ApAccess)]
[ApiController]
[Route("api/ap")]
[Produces("application/json")]
public class AccountsPayableController : ControllerBase
{
    private readonly IAccountsPayableService _svc;
    public AccountsPayableController(IAccountsPayableService svc) => _svc = svc;

    // ── Vendors ───────────────────────────────────────────────────────────────

    [HttpGet("vendors")]
    public async Task<IActionResult> GetVendors(CancellationToken ct)
        => Ok(await _svc.GetVendorsAsync(ct));

    [HttpPost("vendors")]
    [Authorize(Policy = PermissionKeys.ApVendorManage)]
    public async Task<IActionResult> CreateVendor([FromBody] CreateVendorRequest req, CancellationToken ct)
        => StatusCode(201, await _svc.CreateVendorAsync(req, ct));

    [HttpPut("vendors/{id:guid}")]
    [Authorize(Policy = PermissionKeys.ApVendorManage)]
    public async Task<IActionResult> UpdateVendor(Guid id, [FromBody] UpdateVendorRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateVendorAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("vendors/{id:guid}")]
    [Authorize(Policy = PermissionKeys.ApVendorManage)]
    public async Task<IActionResult> DeleteVendor(Guid id, CancellationToken ct)
    {
        try { await _svc.DeleteVendorAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Purchase Orders ───────────────────────────────────────────────────────

    [HttpGet("purchase-orders")]
    public async Task<IActionResult> GetPurchaseOrders(
        [FromQuery] string? status, [FromQuery] Guid? vendorId, CancellationToken ct)
        => Ok(await _svc.GetPurchaseOrdersAsync(status, vendorId, ct));

    [HttpGet("purchase-orders/{id:guid}")]
    public async Task<IActionResult> GetPurchaseOrder(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetPurchaseOrderAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("purchase-orders/{id:guid}/history")]
    public async Task<IActionResult> GetPurchaseOrderHistory(Guid id, CancellationToken ct)
        => Ok(await _svc.GetPurchaseOrderHistoryAsync(id, ct));

    [HttpPost("purchase-orders")]
    [Authorize(Policy = PermissionKeys.ApPurchaseOrderManage)]
    public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreatePurchaseOrderAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/{id:guid}/lines")]
    [Authorize(Policy = PermissionKeys.ApPurchaseOrderManage)]
    public async Task<IActionResult> AddLine(Guid id, [FromBody] AddPOLineRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.AddPOLineAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("purchase-orders/{id:guid}/lines/{lineId:guid}")]
    [Authorize(Policy = PermissionKeys.ApPurchaseOrderManage)]
    public async Task<IActionResult> RemoveLine(Guid id, Guid lineId, CancellationToken ct)
    {
        try { await _svc.RemovePOLineAsync(id, lineId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/{id:guid}/send")]
    [Authorize(Policy = PermissionKeys.ApPurchaseOrderApprove)]
    public async Task<IActionResult> Send(Guid id, CancellationToken ct)
    {
        try { await _svc.SendPurchaseOrderAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Record a goods receipt (partial or full). Can be called multiple times on the same PO.</summary>
    [HttpPost("purchase-orders/{id:guid}/receipts")]
    [Authorize(Policy = PermissionKeys.ApPurchaseOrderReceive)]
    public async Task<IActionResult> RecordReceipt(Guid id, [FromBody] RecordReceiptRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.RecordReceiptAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>List all goods-receipt events for a PO.</summary>
    [HttpGet("purchase-orders/{id:guid}/receipts")]
    public async Task<IActionResult> GetReceipts(Guid id, CancellationToken ct)
        => Ok(await _svc.GetReceiptsAsync(id, ct));

    [HttpPost("purchase-orders/{id:guid}/close")]
    [Authorize(Policy = PermissionKeys.ApPurchaseOrderApprove)]
    public async Task<IActionResult> Close(Guid id, CancellationToken ct)
    {
        try { await _svc.ClosePurchaseOrderAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/{id:guid}/cancel")]
    [Authorize(Policy = PermissionKeys.ApPurchaseOrderApprove)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        try { await _svc.CancelPurchaseOrderAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/{id:guid}/generate-invoice")]
    public async Task<IActionResult> GenerateInvoice(Guid id, [FromQuery] string vendorInvoiceRef, CancellationToken ct)
    {
        try { return Ok(await _svc.GenerateInvoiceFromPOAsync(id, vendorInvoiceRef, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── AP Invoices ───────────────────────────────────────────────────────────

    [HttpGet("invoices")]
    public async Task<IActionResult> GetInvoices([FromQuery] Guid? vendorId, CancellationToken ct)
        => Ok(await _svc.GetInvoicesAsync(vendorId, ct));

    [HttpPost("invoices")]
    [Authorize(Policy = PermissionKeys.ApInvoiceManage)]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateAPInvoiceRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateInvoiceAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Raise a prepayment / advance-payment invoice against a PO before goods arrive.</summary>
    [HttpPost("invoices/prepayment")]
    public async Task<IActionResult> CreatePrepayment([FromBody] CreatePrepaymentInvoiceRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreatePrepaymentInvoiceAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Run three-way match (PO → GRN → Invoice). Returns match result and updates MatchStatus.</summary>
    [HttpPost("invoices/{id:guid}/match")]
    public async Task<IActionResult> RunMatch(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.RunThreeWayMatchAsync(id, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Manager bypass: override a match exception with a documented reason.</summary>
    [HttpPost("invoices/{id:guid}/bypass-match")]
    public async Task<IActionResult> BypassMatch(Guid id, [FromBody] BypassMatchRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.BypassMatchAsync(id, req.Reason, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Apply an existing prepayment invoice to a standard AP invoice (offset).</summary>
    [HttpPost("invoices/{id:guid}/apply-prepayment")]
    public async Task<IActionResult> ApplyPrepayment(Guid id, [FromBody] ApplyPrepaymentRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ApplyPrepaymentAsync(id, req.PrepaymentInvoiceId, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/{id:guid}/approve")]
    [Authorize(Policy = PermissionKeys.ApInvoiceApprove)]
    public async Task<IActionResult> ApproveInvoice(Guid id, CancellationToken ct)
    {
        try { await _svc.ApproveInvoiceAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/{id:guid}/void")]
    [Authorize(Policy = PermissionKeys.ApInvoiceApprove)]
    public async Task<IActionResult> VoidInvoice(Guid id, CancellationToken ct)
    {
        try { await _svc.VoidInvoiceAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── AP Payments ───────────────────────────────────────────────────────────

    [HttpGet("payments")]
    public async Task<IActionResult> GetPayments([FromQuery] Guid? vendorId, CancellationToken ct)
        => Ok(await _svc.GetPaymentsAsync(vendorId, ct));

    [HttpPost("payments")]
    [Authorize(Policy = PermissionKeys.ApInvoiceManage)]
    public async Task<IActionResult> CreatePayment([FromBody] CreateAPPaymentRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreatePaymentAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Reports ───────────────────────────────────────────────────────────────

    [HttpGet("aging")]
    public async Task<IActionResult> GetAging(CancellationToken ct)
        => Ok(await _svc.GetAgingReportAsync(ct));

    [HttpGet("vendors/{id:guid}/ledger")]
    public async Task<IActionResult> GetVendorLedger(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.GetVendorLedgerAsync(id, ct)); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Vendor Addresses ──────────────────────────────────────────────────────

    [HttpGet("vendors/{vendorId:guid}/addresses")]
    public async Task<IActionResult> GetVendorAddresses(Guid vendorId, CancellationToken ct)
        => Ok(await _svc.GetVendorAddressesAsync(vendorId, ct));

    [HttpPost("vendors/{vendorId:guid}/addresses")]
    public async Task<IActionResult> CreateVendorAddress(Guid vendorId, [FromBody] SaveVendorAddressRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.SaveVendorAddressAsync(vendorId, null, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("vendors/{vendorId:guid}/addresses/{addressId:guid}")]
    public async Task<IActionResult> UpdateVendorAddress(Guid vendorId, Guid addressId, [FromBody] SaveVendorAddressRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SaveVendorAddressAsync(vendorId, addressId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("vendors/{vendorId:guid}/addresses/{addressId:guid}")]
    public async Task<IActionResult> DeleteVendorAddress(Guid vendorId, Guid addressId, CancellationToken ct)
    {
        try { await _svc.DeleteVendorAddressAsync(vendorId, addressId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost("vendors/{vendorId:guid}/addresses/{addressId:guid}/set-primary")]
    public async Task<IActionResult> SetPrimaryVendorAddress(Guid vendorId, Guid addressId, CancellationToken ct)
    {
        try { await _svc.SetPrimaryVendorAddressAsync(vendorId, addressId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Vendor Contacts ───────────────────────────────────────────────────────

    [HttpGet("vendors/{vendorId:guid}/contacts")]
    public async Task<IActionResult> GetVendorContacts(Guid vendorId, CancellationToken ct)
        => Ok(await _svc.GetVendorContactsAsync(vendorId, ct));

    [HttpPost("vendors/{vendorId:guid}/contacts")]
    public async Task<IActionResult> CreateVendorContact(Guid vendorId, [FromBody] SaveVendorContactRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.SaveVendorContactAsync(vendorId, null, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("vendors/{vendorId:guid}/contacts/{contactId:guid}")]
    public async Task<IActionResult> UpdateVendorContact(Guid vendorId, Guid contactId, [FromBody] SaveVendorContactRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SaveVendorContactAsync(vendorId, contactId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("vendors/{vendorId:guid}/contacts/{contactId:guid}")]
    public async Task<IActionResult> DeleteVendorContact(Guid vendorId, Guid contactId, CancellationToken ct)
    {
        try { await _svc.DeleteVendorContactAsync(vendorId, contactId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost("vendors/{vendorId:guid}/contacts/{contactId:guid}/set-primary")]
    public async Task<IActionResult> SetPrimaryVendorContact(Guid vendorId, Guid contactId, CancellationToken ct)
    {
        try { await _svc.SetPrimaryVendorContactAsync(vendorId, contactId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── PO Workflow ───────────────────────────────────────────────────────────

    [HttpPost("purchase-orders/{id:guid}/submit-for-approval")]
    public async Task<IActionResult> SubmitPOForApproval(Guid id, [FromBody] SubmitForApprovalRequest req, CancellationToken ct)
    {
        try { await _svc.SubmitPOForApprovalAsync(id, req.SubmittedBy, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/workflow/{workflowInstanceId:guid}/approved")]
    public async Task<IActionResult> POWorkflowApproved(Guid workflowInstanceId, CancellationToken ct)
    {
        try { await _svc.POWorkflowApprovedAsync(workflowInstanceId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/workflow/{workflowInstanceId:guid}/rejected")]
    public async Task<IActionResult> POWorkflowRejected(Guid workflowInstanceId, [FromBody] WorkflowOutcomeRequest req, CancellationToken ct)
    {
        try { await _svc.POWorkflowRejectedAsync(workflowInstanceId, req.Reason ?? string.Empty, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Invoice Workflow ──────────────────────────────────────────────────────

    [HttpPost("invoices/{id:guid}/submit-for-approval")]
    public async Task<IActionResult> SubmitInvoiceForApproval(Guid id, [FromBody] SubmitForApprovalRequest req, CancellationToken ct)
    {
        try { await _svc.SubmitInvoiceForApprovalAsync(id, req.SubmittedBy, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/workflow/{workflowInstanceId:guid}/approved")]
    public async Task<IActionResult> InvoiceWorkflowApproved(Guid workflowInstanceId, CancellationToken ct)
    {
        try { await _svc.InvoiceWorkflowApprovedAsync(workflowInstanceId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/workflow/{workflowInstanceId:guid}/rejected")]
    public async Task<IActionResult> InvoiceWorkflowRejected(Guid workflowInstanceId, CancellationToken ct)
    {
        try { await _svc.InvoiceWorkflowRejectedAsync(workflowInstanceId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Purchase Requisitions ─────────────────────────────────────────────────

    [HttpGet("requisitions")]
    public async Task<IActionResult> GetRequisitions([FromQuery] string? status, CancellationToken ct)
        => Ok(await _svc.GetRequisitionsAsync(status, ct));

    [HttpGet("requisitions/{id:guid}")]
    public async Task<IActionResult> GetRequisition(Guid id, CancellationToken ct)
    {
        var pr = await _svc.GetRequisitionAsync(id, ct);
        return pr is null ? NotFound() : Ok(pr);
    }

    [HttpPost("requisitions")]
    public async Task<IActionResult> CreateRequisition([FromBody] CreatePRRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.CreateRequisitionAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("requisitions/{id:guid}/lines")]
    public async Task<IActionResult> AddPRLine(Guid id, [FromBody] AddPRLineRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.AddPRLineAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("requisitions/{id:guid}/lines/{lineId:guid}")]
    public async Task<IActionResult> RemovePRLine(Guid id, Guid lineId, CancellationToken ct)
    {
        try { await _svc.RemovePRLineAsync(id, lineId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("requisitions/{id:guid}/submit")]
    public async Task<IActionResult> SubmitRequisition(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.SubmitRequisitionAsync(id, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("requisitions/{id:guid}/approve")]
    public async Task<IActionResult> ApproveRequisition(Guid id, [FromBody] SubmitForApprovalRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ApproveRequisitionAsync(id, req.SubmittedBy, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("requisitions/{id:guid}/reject")]
    public async Task<IActionResult> RejectRequisition(Guid id, [FromBody] WorkflowOutcomeRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.RejectRequisitionAsync(id, "system", req.Reason ?? string.Empty, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("requisitions/{id:guid}/convert-to-po")]
    public async Task<IActionResult> ConvertRequisitionToPO(Guid id, [FromBody] ConvertPRToPORequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ConvertRequisitionToPOAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("requisitions/{id:guid}/cancel")]
    public async Task<IActionResult> CancelRequisition(Guid id, CancellationToken ct)
    {
        try { await _svc.CancelRequisitionAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Payment Proposals ─────────────────────────────────────────────────────

    [HttpGet("payment-proposals")]
    public async Task<IActionResult> GetPaymentProposals(CancellationToken ct)
        => Ok(await _svc.GetPaymentProposalsAsync(ct));

    [HttpGet("payment-proposals/{id:guid}")]
    public async Task<IActionResult> GetPaymentProposal(Guid id, CancellationToken ct)
    {
        var p = await _svc.GetPaymentProposalAsync(id, ct);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost("payment-proposals")]
    public async Task<IActionResult> CreatePaymentProposal([FromBody] CreatePaymentProposalRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.CreatePaymentProposalAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("payment-proposals/{id:guid}/lines/{invoiceId:guid}")]
    public async Task<IActionResult> AddProposalLine(Guid id, Guid invoiceId, CancellationToken ct)
    {
        try { return Ok(await _svc.AddProposalLineAsync(id, invoiceId, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("payment-proposals/{id:guid}/lines/{lineId:guid}")]
    public async Task<IActionResult> RemoveProposalLine(Guid id, Guid lineId, CancellationToken ct)
    {
        try { await _svc.RemoveProposalLineAsync(id, lineId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("payment-proposals/{id:guid}/approve")]
    public async Task<IActionResult> ApprovePaymentProposal(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.ApprovePaymentProposalAsync(id, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("payment-proposals/{id:guid}/process")]
    public async Task<IActionResult> ProcessPaymentProposal(Guid id, [FromBody] SubmitForApprovalRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ProcessPaymentProposalAsync(id, req.SubmittedBy, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("payment-proposals/{id:guid}/cancel")]
    public async Task<IActionResult> CancelPaymentProposal(Guid id, CancellationToken ct)
    {
        try { await _svc.CancelPaymentProposalAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Vendor Credit Notes ───────────────────────────────────────────────────

    [HttpGet("credit-notes")]
    public async Task<IActionResult> GetCreditNotes([FromQuery] Guid? vendorId, CancellationToken ct)
        => Ok(await _svc.GetCreditNotesAsync(vendorId, ct));

    [HttpPost("credit-notes")]
    public async Task<IActionResult> CreateCreditNote([FromBody] CreateCreditNoteRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.CreateCreditNoteAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("credit-notes/{id:guid}/post")]
    public async Task<IActionResult> PostCreditNote(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.PostCreditNoteAsync(id, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("credit-notes/{id:guid}/apply/{invoiceId:guid}")]
    public async Task<IActionResult> ApplyCreditNote(Guid id, Guid invoiceId, [FromQuery] decimal amount, CancellationToken ct)
    {
        try { return Ok(await _svc.ApplyCreditNoteAsync(id, invoiceId, amount, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("credit-notes/{id:guid}/void")]
    public async Task<IActionResult> VoidCreditNote(Guid id, CancellationToken ct)
    {
        try { await _svc.VoidCreditNoteAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }
}
