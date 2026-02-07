using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

public class JoinRequestDto
{
    [UsedImplicitly] public int Join_Request_Id { get; set; }
    [UsedImplicitly] public int Bunch_Id { get; set; }
    [UsedImplicitly] public int User_Id { get; set; }
    [UsedImplicitly] public string User_Name { get; set; } = "";
}