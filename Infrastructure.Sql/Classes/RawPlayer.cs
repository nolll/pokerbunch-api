using JetBrains.Annotations;

namespace Infrastructure.Sql.Classes;

public class RawPlayer
{
    [UsedImplicitly] public int Bunch_Id { get; set; }
    [UsedImplicitly] public int Player_Id { get; set; }
    [UsedImplicitly] public int User_Id { get; set; }
    [UsedImplicitly] public string User_Name { get; set; }
    [UsedImplicitly] public string Player_Name { get; set; }
    [UsedImplicitly] public int Role_Id { get; set; }
    [UsedImplicitly] public string Color { get; set; }
}