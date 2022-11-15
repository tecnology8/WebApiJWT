using WebApiJWT.Constants;
using WebApiJWT.Models;

namespace WebApiJWT.Repositories
{
    public static class DataInitializer
    {
        public static void Run(DataContext db)
        {
            if (db.User.Any()) return;

            //Create Role
            var adminRole = new Role { Name = "Administrator" };
            db.Role.Add(adminRole);

            var operatorRole = new Role { Name = "Operator" };
            db.Role.Add(operatorRole);

            db.SaveChanges();


            //Create Privilege
            db.RolePrivilege.Add(new RolePrivilege
            {
                RoleId = adminRole.Id,
                Privilege = PrivilegeConst.ReadUser
            });

            db.RolePrivilege.Add(new RolePrivilege
            {
                RoleId = adminRole.Id,
                Privilege = PrivilegeConst.CreateUser
            });

            db.RolePrivilege.Add(new RolePrivilege
            {
                RoleId = operatorRole.Id,
                Privilege = PrivilegeConst.ReadUser
            });

            db.SaveChanges();


            // create user
            var ruddy = new User
            {
                UserName = "ruddy",
                Password = "ruddy",
                RoleId = adminRole.Id
            };
            db.User.Add(ruddy);

            var jose = new User
            {
                UserName = "jose",
                Password = "jose",
                RoleId = operatorRole.Id
            };
            db.User.Add(jose);

            db.SaveChanges();
        }
    }
}
