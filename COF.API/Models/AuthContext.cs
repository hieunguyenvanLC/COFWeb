using Microsoft.AspNet.Identity.EntityFramework;

namespace CustomOAuthTutorial.API.Models
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("CasptoneProjectContext", throwIfV1Schema: false)
        {
        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }
    }
}