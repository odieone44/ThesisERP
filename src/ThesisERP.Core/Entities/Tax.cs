namespace ThesisERP.Core.Entities;

public class Tax
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public bool IsDeleted { get; set; } = false;
}
