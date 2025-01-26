namespace zKassa_Server.Models
{
    public enum Permission
    {
        // SHOULD NOT CHANGE VALUES, ONLY ADD NEW PERMISSIONS


        //STORE ACTIONS (inherits, so cashiers can store, scos can cashier etc.)

        //Store             10000-10999
        ClockIn = 10_000,
        RequestLeave,

        //Cashier           11000-11999
        CheckoutLogin = 11_000,
        GetProductInfo,
        SubmitTransaction,

        //SCO               12000-12999
        SCOLogin = 12_000,

        //Service           13000-13999
        ServiceLogin = 13_000,
        HandleReturn,
        CustomerOrder,

        //TeamLead          14000-14999
        StockOrder = 14_000,

        // Deliveries?
        // TODO: make good names ^^^

        //Manager           15000-15999
        GrantDenyLeave = 15_000, // Grant/Deny leave for an employee of their store
        RenameEmployee, // Rename an employee of their store in the system
        ChangePrice, // Change the price of an item for a set amount of time in their store

        // HQ ACTIONS

        //HR                16000-16999
        //Admin             17000-17999
        ViewAllEmployees = 17_000, // allow viewing every employee and all their information

        //ProductManager    18000-18999
        ChangeGlobalPrice = 18_000, // change the price of an item for all stores
        CreateProduct,
        UpdateProductAvailability,
        GetDistCenterNames,
        GetDistCenterInfo,
        GetExpandedProductInfo,

        //Developer         19000-19999
        CreateAnyAccount = 19_000, // Create an account with arbitrary properties
        EditAnyEmployee, // Edit any property of any user
    }

    static class PermissionMethods
    {
        public static bool IsAtLeast(this Permission perm, Role role)
        {
            if (role == Role.Developer)
                return true;
            int roleInt = (int)role;
            int reqPermInt = (int)perm;
            int highestPermissionId = ((roleInt + 11) * 1000) - 1;
            if (roleInt <= 5)
                // for store empls check if permission is below or at their level
                return reqPermInt < highestPermissionId;
            int lowestPermissionId = (roleInt + 10) * 1000;
            // for HQ empls only allow access to actions in their role (without inheritance)
            return reqPermInt >= lowestPermissionId && reqPermInt <= highestPermissionId;
        }
    }
}
