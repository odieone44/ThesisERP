namespace ThesisERP.Core.Enums;

public enum DocumentType
{    
    purchase_bill = 2,
    sales_invoice = 3,
    internal_transfer = 4,
    stock_adjustment_plus = 5,
    stock_adjustment_minus = 6,
    purchase_return = 7,
    sales_return = 8
}

public enum OrderType
{
    purchase_order = 0,
    sales_order = 1,
}

public enum TransactionType
{
    document = 0,
    order = 1
}



public enum StockChangeType
{
    positive = 0,
    negative = 1
}

public enum TransactionStatus
{
    draft = 0,
    pending = 1,
    fulfilled = 2,
    closed = 3,
    cancelled = 4
}

public enum TransactionAction
{
    create = 0,
    update = 1,
    fulfill = 2,
    close = 3,
    cancel = 4
}

