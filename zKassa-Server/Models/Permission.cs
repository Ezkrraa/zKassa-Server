namespace zKassa_Server.Models
{
    public enum Permission
    {
        // to be expanded upon
        //STORE ACTIONS
        CheckoutLogin,
        CreateAccount,

        // HQ ACTIONS
        ViewAllEmployees, // allow viewing every employee and all their information
        ChangeEmployee, // allow full changing of any employee property
    }
}
