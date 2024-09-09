//using Microsoft.AspNetCore.Identity;
//using Microsoft.VisualBasic;
//using PROG.Data;

//namespace PROG.Models
//{
//    public class DataMigrationService
//    {
//        private readonly ApplicationDbContext _dbContext;
//        private readonly Information _information;


//        public DataMigrationService(ApplicationDbContext dbContext, Information information)
//        {
//            _dbContext = dbContext;
//            _information = information;
//        }

//        public void MigrateNamesFromInformationToAspNetUsers()
//        {
//            // Retrieve data from Information table
//            var informationData = _dbContext.Information.ToList();

//            // Loop through each Information data item
//            foreach (var informationItem in informationData)
//            {
//                // Create a new instance of the ApplicationUser
//                var user = new ApplicationUser();

//                // Map the data from Information to ApplicationUser
//                user.FirstName = informationItem.FirstName;
//                user.LastName = informationItem.Surname; // Update this line to use the correct property name
//                user.UserName = informationItem.FirstName + "." + informationItem.Surname; // Update this line to use the correct property name
//                user.NormalizedUserName = user.UserName.ToUpper(); // Set the normalized username
//                user.Email = informationItem.Email; // Set the email
//                user.NormalizedEmail = user.Email.ToUpper(); // Set the normalized email

//                // Set a temporary password for the user (this should be changed later)
//                var password = new PasswordHasher<ApplicationUser>();
//                user.PasswordHash = password.HashPassword(user, "TempP@ssw0rd!");

//                // Add the user to the AspNetUsers table
//                _dbContext.Users.Add(user);
//            }

//            // Save the data to the AspNetUsers table
//            _dbContext.SaveChanges();
//        }
//    }
//}