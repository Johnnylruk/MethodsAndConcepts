using Lealthy_Hospital_Application_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Lealthy_Hospital_Application_System.Data;

public class LHASDB : DbContext
{
    public LHASDB(DbContextOptions<LHASDB> options) : base(options)
    {
    }

    public DbSet<StaffModel> Staffs { get; set; }
}

    