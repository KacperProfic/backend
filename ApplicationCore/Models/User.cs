using ApplicationCore.Commons.Repository;

namespace ApplicationCore.Models;

public partial class User: IIdentity<int>
{
    public int Id { get; set; }
}