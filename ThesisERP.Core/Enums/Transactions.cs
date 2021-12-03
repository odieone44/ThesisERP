namespace ThesisERP.Core.Enums
{
    public static class Transactions
    {
        public enum Types
        {
            purchase_order = 0,
            sales_order = 1,
            purchase_bill = 2,
            sales_invoice = 3,
            internal_transfer = 4,
            stock_adjustment_plus = 5,
            stock_adjustment_minus = 6,
            purchase_return = 7,
            sales_return = 8
        }

        public enum Status
        {
            draft = 0,
            pending = 1,
            approved = 2,
            completed = 3,
            cancelled = 4
        }

    }
}
