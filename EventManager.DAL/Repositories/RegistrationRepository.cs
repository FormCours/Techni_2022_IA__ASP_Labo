using Dapper;
using EventManager.DAL.Interfaces;
using EventManager.Domain.Entities;
using System.Data;

namespace EventManager.DAL.Repositories
{
    public class RegistrationRepository : RepositoryBase<int, Registration>, IRegistrationRepository
    {
        public RegistrationRepository(IDbConnection dbConnection) : base(dbConnection)
        { }

        public override IEnumerable<Registration> GetAll()
        {
            connection.Open();
            IEnumerable<Registration> registrations = connection.Query<Registration>(
                "SELECT *" +
                " FROM [V_Registration]"
            );
            connection.Close();

            return registrations;
        }

        public IEnumerable<Registration> GetByMember(int memberId)
        {
            connection.Open();
            IEnumerable<Registration> results = connection.Query<Registration, Member, Activity, Registration>(
                "SELECT *" +
                " FROM [V_Registration]" +
                "  LEFT JOIN [V_Member] ON [V_Registration].[MemberId] = [V_Member].[Id]" +
                "  LEFT JOIN [V_Activity] ON [V_Registration].[ActivityId] = [V_Activity].[Id]" +
                " WHERE [V_Member].[Id] = @MemberId",
                (registration, member, activity) =>
                {
                    registration.Member = member;
                    registration.Activity = activity;
                    return registration;
                },
                new { MemberId = memberId }
            );
            connection.Close();

            return results;
        }

        public IEnumerable<Registration> GetByActivity(int activityId)
        {
            connection.Open();
            IEnumerable<Registration> results = connection.Query<Registration, Member, Activity, Registration>(
                "SELECT *" +
                " FROM [V_Registration]" +
                "  LEFT JOIN [V_Member] ON [V_Registration].[MemberId] = [V_Member].[Id]" +
                "  LEFT JOIN [V_Activity] ON [V_Registration].[ActivityId] = [V_Activity].[Id]" +
                " WHERE [V_Activity].[Id] = @ActivityId",
                (registration, member, activity) =>
                {
                    registration.Member = member;
                    registration.Activity = activity;
                    return registration;
                },
                new { ActivityId = activityId }
            );
            connection.Close();

            return results;
        }
        
        public Registration? GetByActivityAndMember(int activityId, int memberId)
        {
            connection.Open();
            IEnumerable<Registration> results = connection.Query<Registration, Member, Activity, Registration>(
                "SELECT *" +
                " FROM [V_Registration]" +
                "  LEFT JOIN [V_Member] ON [V_Registration].[MemberId] = [V_Member].[Id]" +
                "  LEFT JOIN [V_Activity] ON [V_Registration].[ActivityId] = [V_Activity].[Id]" +
                " WHERE [V_Activity].[Id] = @ActivityId AND [V_Member].[Id] = @MemberId",
                (registration, member, activity) =>
                {
                    registration.Member = member;
                    registration.Activity = activity;
                    return registration;
                },
                new { 
                    ActivityId = activityId,
                    MemberId = memberId
                }
            );
            connection.Close();

            return results.SingleOrDefault();
        }

        public override Registration? GetById(int id)
        {
            connection.Open();
            IEnumerable<Registration> results = connection.Query<Registration, Member, Activity, Registration>(
                "SELECT *" +
                " FROM [V_Registration]" +
                "  LEFT JOIN [V_Member] ON [V_Registration].[MemberId] = [V_Member].[Id]" +
                "  LEFT JOIN [V_Activity] ON [V_Registration].[ActivityId] = [V_Activity].[Id]" +
                " WHERE [V_Registration].[Id] = @Id",
                (registration, member, activity) =>
                {
                    registration.Member = member;
                    registration.Activity = activity;
                    return registration;
                },
                new { Id = id }
            );
            connection.Close();

            return results.SingleOrDefault();
        }

        public override int Insert(Registration entity)
        {
            connection.Open();
            int id = connection.QuerySingle<int>(
                "INSERT INTO [dbo].[Registration]([ActivityId], [MemberId], [NbGuest])" +
                " OUTPUT [Inserted].[Id]" +
                " VALUES(@ActivityId, @MemberId, @NbGuest)",
                entity
            );
            connection.Close();

            return id;
        }

        public override bool Update(Registration entity)
        {
            connection.Open();
            int nbRow = connection.Execute(
                "UPDATE [dbo].[Registration]" +
                " SET [ActivityId] = @ActivityId," +
                "     [MemberId] = @MemberId," +
                "     [NbGuest] = @NbGuest" +
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
                "DELETE FROM [Registration] WHERE Id = @Id",
                new { Id = id }
            );
            connection.Close();

            return nbRow == 1;
        }
    }
}
