using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public class WarehouseLocation : BaseEntity
{
    public Guid    WarehouseId { get; private set; }
    public string  Code        { get; private set; } = string.Empty;
    public string? Zone        { get; private set; }
    public string? Aisle       { get; private set; }
    public string? Bay         { get; private set; }
    public string? Level       { get; private set; }
    public string? Bin         { get; private set; }
    public bool    IsActive    { get; private set; } = true;
    public bool    IsPickable  { get; private set; } = true;
    public bool    IsReceivable{ get; private set; } = true;

    public Warehouse? Warehouse { get; private set; }

    private WarehouseLocation() { }

    public WarehouseLocation(Guid warehouseId, string code,
        string? zone = null, string? aisle = null, string? bay = null,
        string? level = null, string? bin = null,
        bool isPickable = true, bool isReceivable = true)
    {
        WarehouseId  = warehouseId;
        Code         = code.Trim().ToUpper();
        Zone         = zone;
        Aisle        = aisle;
        Bay          = bay;
        Level        = level;
        Bin          = bin;
        IsPickable   = isPickable;
        IsReceivable = isReceivable;
    }

    public void Update(string? zone, string? aisle, string? bay, string? level, string? bin,
        bool isPickable, bool isReceivable)
    {
        Zone         = zone;
        Aisle        = aisle;
        Bay          = bay;
        Level        = level;
        Bin          = bin;
        IsPickable   = isPickable;
        IsReceivable = isReceivable;
        SetUpdated();
    }

    public void Activate()   { IsActive = true;  SetUpdated(); }
    public void Deactivate() { IsActive = false; SetUpdated(); }
}
