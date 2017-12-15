namespace ImprovementOpportunityApp.Migrations
{
    using ImprovementOpportunityApp.AppCommons;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ImprovementOpportunityApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(ImprovementOpportunityApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            IdentityRole employeeRole = roleManager.FindByName(ApplicationRoles.EMPLOYEE);
            IdentityRole deptRole = roleManager.FindByName(ApplicationRoles.DEPARTMENT_HEAD);
            IdentityRole adminRole = roleManager.FindByName(ApplicationRoles.FIRM_ADMIN);

            if (employeeRole == null)
            {
                employeeRole = new IdentityRole
                {
                    Name = ApplicationRoles.EMPLOYEE
                };
                roleManager.Create(employeeRole);
            }

            if (deptRole == null)
            {
                deptRole = new IdentityRole
                {
                    Name = ApplicationRoles.DEPARTMENT_HEAD
                };
                roleManager.Create(deptRole);
            }

            if (adminRole == null)
            {
                adminRole = new IdentityRole
                {
                    Name = ApplicationRoles.FIRM_ADMIN
                };
                roleManager.Create(adminRole);
            }

            Models.Data.Department departmentIT = context.Departments.FirstOrDefault(d => d.Name.ToLower() == "IT Department");
            if (departmentIT == null)
            {
                departmentIT = context.Departments.Add(new Models.Data.Department
                {
                    Name = "IT Department"
                });
            }
            context.SaveChanges();
            
            //Models.Data.Department department = context.Departments.Add(new Models.Data.Department
            //{
            //    Name = "Medical Research"
            //});

            //Models.Data.Topic topic = context.Topics.Add(new Models.Data.Topic
            //{
            //    Name = "Accessories"
            //});

            //Models.ApplicationUser userQuinee = context.Users.Add(new Models.ApplicationUser
            //{
            //    Department = department,
            //    Email = "quinee@gmail.com",
            //    FirstName = "Quincy",
            //    LastName = "Amin",
            //    PhoneNumber = "4166124124",
            //    UserName = "quinee@gmail.com"
            //});

            //Models.ApplicationUser userHarhil = context.Users.Add(new Models.ApplicationUser
            //{
            //    Department = department,
            //    Email = "harshil@gmail.com",
            //    FirstName = "Harshil",
            //    LastName = "Mehta",
            //    PhoneNumber = "4166124124",
            //    UserName = "harshil@gmail.com"
            //});

            //Models.ApplicationUser userDept = context.Users.Add(new Models.ApplicationUser
            //{
            //    Department = departmentIT,
            //    Email = "alex@gmail.com",
            //    FirstName = "Alex",
            //    LastName = "L",
            //    PhoneNumber = "4166124124",
            //    UserName = "alex@gmail.com"
            //});
            var userManger = new UserManager<Models.ApplicationUser>(new UserStore<Models.ApplicationUser>(context));
            Models.ApplicationUser userAdmin = userManger.FindByName("pragnesh@gmail.com");

            if (userAdmin == null)
            {
                userAdmin = context.Users.Add(new Models.ApplicationUser
                {
                    Department = departmentIT,
                    Email = "pragnesh@gmail.com",
                    FirstName = "Pragnesh",
                    LastName = "Patel",
                    PhoneNumber = "4166124124",
                    UserName = "pragnesh@gmail.com"
                });

                userManger.Create(userAdmin, "Pragnesh123!");
                userManger.AddToRole(userAdmin.Id, ApplicationRoles.FIRM_ADMIN);
            }

            context.SaveChanges();

            ////var adminEmail = ConfigurationManager.AppSettings["AdminEmail"];
            //userManger.Create(userQuinee, "Quinee123!");
            //userManger.AddToRole(userQuinee.Id, ApplicationRoles.EMPLOYEE);

            //userManger.Create(userHarhil, "Harshil123!");
            //userManger.AddToRole(userHarhil.Id, ApplicationRoles.EMPLOYEE);

            //userManger.Create(userDept, "Alex123!");
            //userManger.AddToRole(userDept.Id, ApplicationRoles.DEPARTMENT_HEAD);

            //Models.Data.Suggestion quineeSuggestion = context.Suggestions.Add(new Models.Data.Suggestion
            //{
            //    Department = department,
            //    Title = "Please provide new grade A accessories",
            //    Description = "dsad asd sad asd as das dsa dadas",
            //    Topic = topic,
            //    User = userQuinee,
            //    HasReviewed = true,
            //    HasConsidered = true
            //});

            //Models.Data.Suggestion hasrshilSuggestion = context.Suggestions.Add(new Models.Data.Suggestion
            //{
            //    Department = department,
            //    Title = "Please provide new hand sanatizers at each office room",
            //    Description = "dsad asd sad asd as das dsa dadas",
            //    Topic = topic,
            //    User = userHarhil,
            //    HasReviewed = true,
            //    HasConsidered = true
            //});

            //Models.Data.Forum forum1 = context.Forums.Add(new Models.Data.Forum
            //{
            //    Suggestion = hasrshilSuggestion,
            //    Department = department,
            //    Topic = topic
            //});

            //Models.Data.ForumMessage forumMessage1 = context.ForumMessages.Add(new Models.Data.ForumMessage
            //{
            //    Forum = forum1,
            //    ReplyMessageId = null,
            //    Message = "dasd sad sa dadasdasdas",
            //    User = userQuinee
            //});

            //context.SaveChanges();

            //Models.Data.ForumMessage forumMessage2 = context.ForumMessages.Add(new Models.Data.ForumMessage
            //{
            //    Forum = forum1,
            //    ReplyMessageId = forumMessage1.ForumMessageId,
            //    Message = "gfdg dfdf gdfgdfgfd",
            //    User = userHarhil
            //});

            //Models.Data.ForumMessage forumMessage3 = context.ForumMessages.Add(new Models.Data.ForumMessage
            //{
            //    Forum = forum1,
            //    ReplyMessageId = null,
            //    Message = "rrewr werwerwerweteww re",
            //    User = userHarhil
            //});

            //Models.Data.ForumMessage forumMessage4 = context.ForumMessages.Add(new Models.Data.ForumMessage
            //{
            //    Forum = forum1,
            //    ReplyMessageId = forumMessage2.ForumMessageId,
            //    Message = "vbc bvc bcv vb bcvbvbvc",
            //    User = userHarhil
            //});

            //Models.Data.Forum forum2 = context.Forums.Add(new Models.Data.Forum
            //{
            //    Suggestion = quineeSuggestion,
            //    Department = department,
            //    Topic = topic
            //});

            //context.SaveChanges(); 
        }
    }
}
