namespace ThesisERP.Core.Enums;

public enum DocumentType
{    
    purchase_bill = 1,
    sales_invoice = 2,    
    stock_adjustment_plus = 3,
    stock_adjustment_minus = 4,
    purchase_return = 5,
    sales_return = 6
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

