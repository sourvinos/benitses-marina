using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Expenses.DocumentTypes {

    public class DocumentTypeValidation : Repository<DocumentType>, IDocumentTypeValidation {

        public DocumentTypeValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(DocumentType z, DocumentTypeWriteDto documentType) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, documentType) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(DocumentType z, DocumentTypeWriteDto documentType) {
            return z != null && z.PutAt != documentType.PutAt;
        }

    }

}