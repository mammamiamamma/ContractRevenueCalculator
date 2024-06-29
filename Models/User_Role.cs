using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBDProject.Models;

[PrimaryKey(nameof(Role_Id), nameof(User_Id))]
public class User_Role
{
    public int Role_Id { get; set; }
    [ForeignKey(nameof(Role_Id))]
    public Role Role { get; set; }
    public int User_Id { get; set; }
    [ForeignKey(nameof(User_Id))]
    public User User { get; set; }
}