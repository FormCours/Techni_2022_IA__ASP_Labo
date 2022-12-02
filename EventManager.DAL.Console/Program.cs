using EventManager.DAL.Interfaces;
using EventManager.DAL.Repositories;
using EventManager.Domain.Entities;
using System.Data;
using System.Data.SqlClient;

// Création de la connection
IDbConnection connection = new SqlConnection("Data Source=desktop-6a08int;Initial Catalog=EventManager;Integrated Security=True");

// Instanciation des repo
IMemberRepository memberRepository = new MemberRepository(connection);
IActivityRepository activityRepository = new ActivityRepository(connection);
IRegistrationRepository registrationRepository = new RegistrationRepository(connection);


// Member
Console.WriteLine("> Liste des membres");
Console.WriteLine("[");
IEnumerable<Member> members = memberRepository.GetAll();
foreach (Member member in members)
{
    Console.WriteLine(member);
}
Console.WriteLine("]");
Console.WriteLine();

Console.WriteLine("> Member by Id (1)");
Member? member1 = memberRepository.GetById(1);
Console.WriteLine(member1);
Console.WriteLine();

Console.WriteLine("> Member by Pseudo (Zaza)");
Member? member2 = memberRepository.GetByIdentifier("zaza");
Console.WriteLine(member2);
Console.WriteLine();

Console.WriteLine("> Member by Email (zaza.vanderquak@demo.be)");
Member? member3 = memberRepository.GetByIdentifier("zaza.vanderquak@demo.be");
Console.WriteLine(member3);
Console.WriteLine();


// Activity
Console.WriteLine("> Liste des activities");
Console.WriteLine("[");
IEnumerable<Activity> activities = activityRepository.GetAll();
foreach (Activity activity in activities)
{
    Console.WriteLine(activity);
}
Console.WriteLine("]");
Console.WriteLine();

Console.WriteLine("> Activity by Id (1)");
Activity? activity1 = activityRepository.GetById(1);
Console.WriteLine(activity1);
Console.WriteLine();


// Registration
Console.WriteLine("> Liste des activities");
Console.WriteLine("[");
IEnumerable<Registration> registrations = registrationRepository.GetAll();
foreach (Registration registration in registrations)
{
    Console.WriteLine(registration);
}
Console.WriteLine("]");
Console.WriteLine();

Console.WriteLine("> Registration by Id (1)");
Registration? registration1 = registrationRepository.GetById(1);
Console.WriteLine(registration1);
Console.WriteLine();
