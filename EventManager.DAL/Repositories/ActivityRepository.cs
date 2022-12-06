using Dapper;
using EventManager.DAL.Interfaces;
using EventManager.Domain.Entities;
using System.Data;

namespace EventManager.DAL.Repositories
{
    public class ActivityRepository : RepositoryBase<int, Activity>, IActivityRepository
    {
        public ActivityRepository(IDbConnection dbConnection) : base(dbConnection)
        { }

        public override IEnumerable<Activity> GetAll()
        {
            connection.Open();
            IEnumerable<Activity> activities = connection.Query<Activity>(
                "SELECT *" +
                " FROM [Activity]" +
                " WHERE [IsCancel] = 0"
            );
            connection.Close();

            return activities;
        }

        public IEnumerable<Activity> GetAll(bool includePast)
        {
            if(includePast)
            {
                return GetAll();
            }

            connection.Open();
            IEnumerable<Activity> activities = connection.Query<Activity>(
                "SELECT *" +
                " FROM [Activity]" +
                " WHERE [EndDate] >= GETDATE() AND [IsCancel] = 0"
            );
            connection.Close();

            return activities;
        }


        public override Activity? GetById(int id)
        {
            connection.Open();
            IEnumerable<Activity> results = connection.Query<Activity, Member, Activity>(
                "SELECT *" +
                " FROM [V_Activity]" +
                "  LEFT JOIN [V_Member] ON [V_Activity].[CreatorId] = [V_Member].[Id]" +
                " WHERE [V_Activity].[Id] = @Id",
                (activity, member) => {
                    activity.Creator = member;
                    return activity; 
                },
                new { Id = id }
            );
            connection.Close();

            return results.SingleOrDefault();
        }

        public override int Insert(Activity entity)
        {
            connection.Open();
            int id = connection.QuerySingle<int>(
                "INSERT INTO [dbo].[Activity]([Name],[Description],[StartDate],[EndDate],[ImageName],[ImageSrc],[MaxGuest],[CreatorId],[IsCancel])" +
                " OUTPUT[Inserted].[Id]" +
                " VALUES(@Name, @Description, @StartDate, @EndDate, @ImageName, @ImageSrc, @MaxGuest, @CreatorId, @IsCancel)",
                entity
            );
            connection.Close();

            return id;
        }

        public override bool Update(Activity entity)
        {
            connection.Open();
            int nbRow = connection.Execute(
                "UPDATE [dbo].[Activity]" +
                " SET [Name] = @Name, " +
                "     [Description] = @Description, " +
                "     [StartDate] = @StartDate, " +
                "     [EndDate] = @EndDate, " +
                "     [ImageName] = @ImageName, " +
                "     [ImageSrc] = @ImageSrc, " +
                "     [MaxGuest] = @MaxGuest, " +
                "     [IsCancel] = @IsCancel" +
                " WHERE [Id] = @Id",
                entity
            );
            connection.Close();

            return nbRow == 1;
        }

        public override bool Delete(int id)
        {
            connection.Open();
            int nbRow = connection.Execute(
                "DELETE FROM [Activity] WHERE Id = @Id",
                new { Id = id }
            );
            connection.Close();

            return nbRow == 1;
        }
    }
}
