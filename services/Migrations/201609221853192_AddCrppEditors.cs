namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCrppEditors : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into [dbo].[ProjectUsers]([Project_Id], [User_Id])
values (2247, 3118), --BambiH
(2247, 3119), -- ShawnS
(2247, 3120), -- CatherineD
(2247, 3121), -- CareyM
(2247, 3122), -- JenniferK
(2247, 3123), -- ShariS
(2247, 3124), -- ArthurVP

(2247, 1054), -- AmyS
(2247, 3125), -- HollyB
(2247, 3126) -- WilburB

--update dbo.Users set Description = 'Archeologist I' where Id = 1054
--update dbo.Users set DepartmentId = 1015 where Id = 1054
--update dbo.Users set Roles = '[""CRPP""]' where Id = 1054
            ");
        }

        public override void Down()
        {
            Sql(@"
delete from [dbo].[ProjectUsers]
where
Project_Id = 2247
and
(
[User_Id] = 3103
or [User_Id] = 3118
or [User_Id] = 3119
or [User_Id] = 3120
or [User_Id] = 3121
or [User_Id] = 3122
or [User_Id] = 3123
or [User_Id] = 3124

--or [User_Id] = 1054
)
            ");
        }
    }
}
