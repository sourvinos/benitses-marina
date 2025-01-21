using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.DocumentTypes {

    public class DocumentTypeValidation : Repository<SaleDocumentType>, ISaleDocumentTypeValidation {

        public DocumentTypeValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(SaleDocumentType z, SaleDocumentTypeWriteDto documentType) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, documentType) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(SaleDocumentType z, SaleDocumentTypeWriteDto DocumentType) {
            return z != null && z.PutAt != DocumentType.PutAt;
        }

    }

}