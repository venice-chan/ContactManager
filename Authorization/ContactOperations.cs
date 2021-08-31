using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ContactManager.Authorization
{
  public static class ContactOperations
  {
    public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = ContactOperationName.Create };
    public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = ContactOperationName.Read };
    public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = ContactOperationName.Update };
    public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = ContactOperationName.Delete };
    public static OperationAuthorizationRequirement Approve = new OperationAuthorizationRequirement { Name = ContactOperationName.Approve };
    public static OperationAuthorizationRequirement Reject = new OperationAuthorizationRequirement { Name = ContactOperationName.Reject };

  }

  public static class ContactOperationName
  {
    public static readonly string Create = "Create";
    public static readonly string Read = "Read";
    public static readonly string Update = "Update";
    public static readonly string Delete = "Delete";
    public static readonly string Approve = "Approve";

    public static readonly string Reject = "Reject";

  }
}