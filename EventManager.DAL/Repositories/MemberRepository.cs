using Dapper;
using EventManager.DAL.Interfaces;
using EventManager.Domain.Entities;
using System.Data;

namespace EventManager.DAL.Repositories
{
    public class MemberRepository : RepositoryBase<int, Member>, IMemberRepository
    {
        public MemberRepository(IDbConnection dbConnection) : base(dbConnection)
        { }

        public override IEnumerable<Member> GetAll()
        {
            connection.Open();
            IEnumerable<Member> members = connection.Query<Member>(
                "SELECT *" +
                " FROM [V_Member]"
            );
            connection.Close();

            return members;
        }

        public override Member? GetById(int id)
        {
            connection.Open();
            Member member = connection.QuerySingleOrDefault<Member>(
                "SELECT *" +
                " FROM [V_Member]" +
                " WHERE [Id] = @Id",
                new { Id = id }
            );
            connection.Close();

            return member;
        }

        public Member? GetByIdentifier(string identifier)
        {
            connection.Open();
            Member member = connection.QuerySingleOrDefault<Member>(
                "SELECT *" +
                " FROM [V_Member]" +
                " WHERE [Pseudo] = @Identifier OR [Email] = @Identifier",
                new { Identifier = identifier }
            );
            connection.Close();

            return member;
        }

        public string? GetHashPwd(int id)
        {
            connection.Open();
            string hashPwd = connection.QuerySingleOrDefault<string>(
                "SELECT [HashPwd]" +
                " FROM [Member]" +
                " WHERE [Id] = @Id",
                new { Id = id }
            );
            connection.Close();

            return hashPwd;
        }

        public bool EmailExists(string email)
        {
            connection.Open();
            int count = connection.ExecuteScalar<int>(
                "SELECT COUNT(*)" +
                " FROM [Member]" +
                " WHERE Email = @Email",
                new { Email = email }
            );
            connection.Close();

            return count != 0;
        }

        public bool PseudoExists(string pseudo)
        {
            connection.Open();
            int count = connection.ExecuteScalar<int>(
                "SELECT COUNT(*)" +
                " FROM [Member]" +
                " WHERE Pseudo = @Pseudo",
                new { Pseudo = pseudo }
            );
            connection.Close();

            return count != 0;
        }

        public override int Insert(Member entity)
        {
            connection.Open();
            int id = connection.QuerySingle<int>(
                "INSERT INTO [dbo].[Member]([Pseudo], [Email], [HashPwd], [Firstname], [Lastname], [Birthdate])" +
                " OUTPUT [Inserted].[Id]" +
                " VALUES(@Pseudo, @Email, @HashPwd, @Firstname, @Lastname, @Birthdate)",
                entity
            );
            connection.Close();

            return id;
        }

        public override bool Update(Member entity)
        {
            connection.Open();
            int nbRow = connection.Execute(
                "UPDATE [dbo].[Member]" +
                " SET [Pseudo] = @Pseudo," +
                "     [Email] = @Email," +
                "     [Firstname] = @Firstname," +
                "     [Lastname] = @Lastname," +
                "     [Birthdate] = @Birthdate" +
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
                "DELETE FROM [Member] WHERE Id = @Id",
                new { Id = id }
            );
            connection.Close();

            return nbRow == 1;
        }
    }
}
