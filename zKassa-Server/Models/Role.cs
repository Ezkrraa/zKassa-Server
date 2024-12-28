namespace zKassa_Server.Models
{
    public enum Role
    {
        // STORE ROLES (inherits)
        Store, // No access to cashier systems
        Cashier, // Basic cashiering transactions, no returns
        SCO, // Self CheckOut (cashier + self-checkout)
        Service, // Cashier + returns, customer orders
        TeamLead, // Service + dropping receipts/transactions, pricing changes
        Manager, // stock orders, deliveries, write-offs shop use, HR lite

        // HQ (no inheritance)
        HR, // duhh (no inherited perms)
        Admin, // access to store roles up to teamLead, (TO BE ADDED ON)
        ProductManager, // full access to product properties (inl. creation)
        Dev, // full access (duhh)
    }
}
