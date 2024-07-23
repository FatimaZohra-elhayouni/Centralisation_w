namespace CentralisationV0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationDataCollab : DbMigration
    {
        public override void Up()
        {
           


           
        }


        public override void Down()
        {
            DropForeignKey("public.CollaborationDatas", "Data_IdData", "public.Data");
            DropForeignKey("public.CollaborationDatas", "Collaboration_idCollaborateur", "public.Collaborations");
            DropForeignKey("public.Collaborations", "idTypeCollaboration", "public.TypeCollaborations");
            DropForeignKey("public.Collaborations", "idClient", "public.Clients");

            DropIndex("public.CollaborationDatas", new[] { "Data_IdData" });
            DropIndex("public.CollaborationDatas", new[] { "Collaboration_idCollaborateur" });
            DropIndex("public.Collaborations", new[] { "idTypeCollaboration" });
            DropIndex("public.Collaborations", new[] { "idClient" });

            DropTable("public.CollaborationDatas");
            DropTable("public.Collaborations");
        }

    }
}
