using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Features.Reservations;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Implementations;
using API.Infrastructure.Users;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace API.Features.Leases {

    public class LeasePdfRepository : Repository<Reservation>, ILeasePdfRepository {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly UserManager<UserExtended> userManager;

        public LeasePdfRepository(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContextAccessor, settings, userManager) {
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<LeasePdfVM> GetByIdAsync(string reservationId) {
            var x = await context.Reservations
                .AsNoTracking()
                .Include(x => x.Boat).ThenInclude(x => x.Type)
                .Include(x => x.Boat).ThenInclude(x => x.Usage)
                .Include(x => x.Insurance)
                .Include(x => x.Owner)
                .Include(x => x.Billing)
                .Include(x => x.Fee)
                .Where(x => x.ReservationId.ToString() == reservationId)
                .SingleOrDefaultAsync();
            return mapper.Map<Reservation, LeasePdfVM>(x);
        }

        public string BuildLeasePdf(LeasePdfVM lease) {
            var document = new Document();
            Style style = document.Styles["Normal"];
            style.Font.Name = "Verdana";
            var section = document.AddSection();
            section.PageSetup.TopMargin = 20;
            section.PageSetup.LeftMargin = 30;
            LogoAndCompany(section);
            Spacer(section);
            Header(section);
            Spacer(section);
            VesselNameAndFlag(lease.Boat, section);
            PortAndVesselRegistryNo(lease.Boat, section);
            VesselDimensions(lease.Boat, section);
            VesselTypeAndUse(lease.Boat, section);
            MooringPeriod(lease.Period, section);
            Spacer(section);
            InsuranceHeaders(section);
            InsuranceDetails(lease.Insurance, section);
            // PolicyNoAndExpireDate(lease.Insurance, section);
            Spacer(section);
            PersonHeaders(section);
            PersonNames(lease.Owner, lease.Billing, section);
            PersonAddresses(lease.Owner, lease.Billing, section);
            PersonVATs(lease.Owner, lease.Billing, section);
            PersonTaxOffices(lease.Owner, lease.Billing, section);
            PersonIdNumbers(lease.Owner, lease.Billing, section);
            PersonPhoneNumbers(lease.Owner, lease.Billing, section);
            PersonEmails(lease.Owner, lease.Billing, section);
            Spacer(section);
            CaptainHeaders(section);
            CaptainDetails(section);
            Spacer(section);
            FeeHeaders(section);
            FeeDetails(lease.Fee, section);
            Spacer(section);
            SmallTerms(section);
            Spacer(section);
            SignatureHeaders(section);
            Signatures(section);
            Username(section);
            TermsAndConditions(document, section);
            return SavePdf(document, lease.ReservationId.ToString());
        }

        public FileStreamResult OpenPdf(string filename) {
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + filename);
            byte[] byteArray = File.ReadAllBytes(fullpathname);
            MemoryStream memoryStream = new(byteArray);
            return new FileStreamResult(memoryStream, "application/pdf");
        }

        private static Row LogoAndCompany(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0;
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("6cm");
            table.AddColumn("11cm");
            Row row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph().AddImage("Images/Background.jpg").Width = "2.5cm";
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph("MARITIME PROJECT Ι.Κ.Ε.").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("ΜΠΕΝΙΤΣΕΣ, 49084, ΚΕΡΚΥΡΑ").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Α.Φ.Μ. 801515394  Δ.Ο.Υ. ΚΕΡΚΥΡΑΣ").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Αρ. Γ.E.ΜΗ. 158323233000").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("ΤΗΛ. +30 26610 72627").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("https://benitsesmarina.com").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Email: info@benitsesmarina.com").Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row Header(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("19cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 7;
            row.BottomPadding = 7;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("ΣΥΜΒΑΣΗ ΠΑΡΑΜΟΝΗΣ ΣΚΑΦΟΥΣ\nVESSEL'S BERTH LEASE AGREEMENT");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row VesselNameAndFlag(LeasePdfBoatVM boat, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ονομα σκάφους");
            row.Cells[0].AddParagraph("Vessel's name");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(boat.Name);
            row.Cells[2].AddParagraph("Σημαία");
            row.Cells[2].AddParagraph("Flag");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(boat.Flag);
            return row;
        }

        private static Row PortAndVesselRegistryNo(LeasePdfBoatVM boat, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Λιμάνι νηολόγησης");
            row.Cells[0].AddParagraph("Port of registry");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(boat.RegistryPort);
            row.Cells[2].AddParagraph("Αρ. νηολογίου");
            row.Cells[2].AddParagraph("Registry No");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(boat.RegistryNo);
            return row;
        }

        private static Row VesselDimensions(LeasePdfBoatVM boat, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ολικό μήκος");
            row.Cells[0].AddParagraph("Overall length");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(boat.Loa + "m");
            row.Cells[2].AddParagraph("Μέγιστο πλάτος");
            row.Cells[2].AddParagraph("Max beam");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(boat.Beam + "m");
            row.Cells[4].AddParagraph("Μέγιστο βύθισμα");
            row.Cells[4].AddParagraph("Max draft");
            row.Cells[4].Borders.Right.Clear();
            row.Cells[5].AddParagraph(boat.Draft + "m");
            return row;
        }

        private static Row VesselTypeAndUse(LeasePdfBoatVM boat, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Τύπος σκάφους");
            row.Cells[0].AddParagraph("Vessel type");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(boat.Type.Description);
            row.Cells[2].AddParagraph("Χρήση σκάφους");
            row.Cells[2].AddParagraph("Vessel use");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(boat.Usage.Description);
            return row;
        }

        private static Row MooringPeriod(LeasePdfPeriodVM period, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Εναρξη ελλιμενισμού");
            row.Cells[0].AddParagraph("Mooring from");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(DateHelpers.FormatDateStringToLocaleString(period.FromDate));
            row.Cells[2].AddParagraph("Λήξη ελλιμενισμού");
            row.Cells[2].AddParagraph("Mooring until");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(DateHelpers.FormatDateStringToLocaleString(period.ToDate));
            return row;
        }

        private static Row InsuranceHeaders(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("19cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Στοιχεία ασφάλισης / Insurance details");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row InsuranceDetails(LeasePdfInsuranceVM insurance, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            table.AddColumn("3.17cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255,255,255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ασφαλιστική εταιρία");
            row.Cells[0].AddParagraph("Insurance company");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(insurance.InsuranceCompany);
            row.Cells[2].AddParagraph("Νο συμβολαίου");
            row.Cells[2].AddParagraph("Policy number");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(insurance.PolicyNo);
            row.Cells[4].AddParagraph("Ημερ/νία λήξης");
            row.Cells[4].AddParagraph("Valid until");
            row.Cells[4].Borders.Right.Clear();
            row.Cells[5].AddParagraph(DateHelpers.FormatDateStringToLocaleString(insurance.PolicyEnds));
            return row;
        }

        // private static Row InsuranceCompany(LeasePdfInsuranceVM insurance, Section section) {
        //     var table = section.AddTable();
        //     table.Borders.Width = 0.1;
        //     table.Borders.Color = new Color(153, 162, 165);
        //     table.Format.Font.Size = Unit.FromCentimeter(0.25);
        //     table.AddColumn("3.5cm");
        //     table.AddColumn("15.5cm");
        //     Row row = table.AddRow();
        //     row.Shading.Color = new Color(255, 255, 255);
        //     row.TopPadding = 2;
        //     row.BottomPadding = 2;
        //     row.VerticalAlignment = VerticalAlignment.Center;
        //     row.Cells[0].AddParagraph("Ασφαλιστική εταιρία");
        //     row.Cells[0].AddParagraph("Insurance company");
        //     row.Cells[0].Borders.Right.Clear();
        //     row.Cells[1].AddParagraph(insurance.InsuranceCompany);
        //     return row;
        // }

        // private static Row PolicyNoAndExpireDate(LeasePdfInsuranceVM insurance, Section section) {
        //     var table = section.AddTable();
        //     table.Borders.Width = 0.1;
        //     table.Borders.Color = new Color(153, 162, 165);
        //     table.Format.Font.Size = Unit.FromCentimeter(0.25);
        //     table.AddColumn("3.5cm");
        //     table.AddColumn("6cm");
        //     table.AddColumn("3.5cm");
        //     table.AddColumn("6cm");
        //     Row row = table.AddRow();
        //     row.Shading.Color = new Color(218, 238, 243);
        //     row.TopPadding = 2;
        //     row.BottomPadding = 2;
        //     row.VerticalAlignment = VerticalAlignment.Center;
        //     row.Cells[0].AddParagraph("Αρ. ασφαλιστηρίου");
        //     row.Cells[0].AddParagraph("Policy No");
        //     row.Cells[0].Borders.Right.Clear();
        //     row.Cells[1].AddParagraph(insurance.PolicyNo);
        //     row.Cells[2].AddParagraph("Ημερ/νία λήξης");
        //     row.Cells[2].AddParagraph("Valid until");
        //     row.Cells[2].Borders.Right.Clear();
        //     row.Cells[3].AddParagraph(DateHelpers.FormatDateStringToLocaleString(insurance.PolicyEnds));
        //     return row;
        // }

        private static Row PersonHeaders(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("9.5cm");
            table.AddColumn("9.5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Στοιχεία πλοιοκτήτη / Owner's details");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Στοιχεία τιμολόγησης / Invoice details");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row PersonNames(LeasePdfPersonVM owner, LeasePdfPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ονοματεπώνυμο");
            row.Cells[0].AddParagraph("Full name");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(owner.Name);
            row.Cells[2].AddParagraph("Ονοματεπώνυμο");
            row.Cells[2].AddParagraph("Full name");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(billing.Name);
            return row;
        }

        private static Row PersonAddresses(LeasePdfPersonVM owner, LeasePdfPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Διεύθυνση κατοικίας");
            row.Cells[0].AddParagraph("Home address");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(owner.Address);
            row.Cells[2].AddParagraph("Διεύθυνση κατοικίας");
            row.Cells[2].AddParagraph("Home address");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(billing.Address);
            return row;
        }

        private static Row PersonVATs(LeasePdfPersonVM owner, LeasePdfPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("ΑΦΜ");
            row.Cells[0].AddParagraph("Tax No");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(owner.TaxNo);
            row.Cells[2].AddParagraph("Α.Φ.Μ.");
            row.Cells[2].AddParagraph("Tax No");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(billing.TaxNo);
            return row;
        }

        private static Row PersonTaxOffices(LeasePdfPersonVM owner, LeasePdfPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Δ.Ο.Υ.");
            row.Cells[0].AddParagraph("Tax Office");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(owner.TaxOffice);
            row.Cells[2].AddParagraph("Δ.Ο.Υ.");
            row.Cells[2].AddParagraph("Tax Office");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(billing.TaxOffice);
            return row;
        }

        private static Row PersonIdNumbers(LeasePdfPersonVM owner, LeasePdfPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("ΑΦΜ");
            row.Cells[0].AddParagraph("Tax No");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(owner.TaxNo);
            row.Cells[2].AddParagraph("ΑΦΜ");
            row.Cells[2].AddParagraph("Tax No");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(billing.TaxNo);
            return row;
        }

        private static Row PersonPhoneNumbers(LeasePdfPersonVM owner, LeasePdfPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Αρ. Τηλεφώνου");
            row.Cells[0].AddParagraph("Phone numbers");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(owner.Phones);
            row.Cells[2].AddParagraph("Αρ. Τηλεφώνου");
            row.Cells[2].AddParagraph("Phone number");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(billing.Phones);
            return row;
        }

        private static Row PersonEmails(LeasePdfPersonVM owner, LeasePdfPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 7;
            row.BottomPadding = 7;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Email");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(owner.Email);
            row.Cells[2].AddParagraph("Email");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(billing.Email);
            return row;
        }

        private static Row CaptainHeaders(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("19cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Στοιχεία πλοιάρχου / Captain's details");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row CaptainDetails(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3.5cm");
            table.AddColumn("6cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ονοματεπώνυμο");
            row.Cells[0].AddParagraph("Full name");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("");
            row.Cells[2].AddParagraph("Αρ. Τηλεφώνου");
            row.Cells[2].AddParagraph("Phone number");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("");
            return row;
        }

        private static Row FeeHeaders(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("19cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Οικονομικά στοιχεία / Fees and taxes");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row FeeDetails(LeasePdfFeeVM fee, Section section) {
            var locale = CultureInfo.CreateSpecificCulture("el-GR");
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("14cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Συμφωνηθέν τίμημα / Initial fee");
            row.Cells[0].AddParagraph("Εκπτωση % / Discount %");
            row.Cells[0].AddParagraph("Ποσό έκπτωσης / Discount amount");
            row.Cells[0].AddParagraph("Υπόλοιπο τιμήματος / Remaining fee");
            row.Cells[0].AddParagraph("ΦΠΑ " + fee.VatPercent + "% / VAT " + fee.VatPercent + "%");
            row.Cells[1].AddParagraph(fee.NetAmount.ToString("N2", locale)).Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph("0,00").Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph("0,00").Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph(fee.NetAmount.ToString("N2", locale)).Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph(fee.VatAmount.ToString("N2", locale)).Format.Alignment = ParagraphAlignment.Right;
            row = table.AddRow();
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.Cells[0].AddParagraph("Τελικό τίμημα προς πληρωμή / Final fee");
            row.Cells[1].AddParagraph("EUR " + fee.GrossAmount.ToString("N2", locale)).Format.Alignment = ParagraphAlignment.Right;
            return row;
        }

        private static Row SmallTerms(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(7);
            table.AddColumn("9.5cm");
            table.AddColumn("9.5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ο πλοιοκτήτης δηλώνει ότι έλαβε γνώση όλων των όρων του ισχύοντος Ειδικού Κανονισμού Λειτουργίας της Μαρίνας (απόσπασμα του οποίου προσαρτάται στην παρούσα) και του ισχύοντος τιμοκαταλόγου, τους οποίους αποδέχεται ανεπιφύλακτα και οι οποίοι θα διέπουν την παρούσα.");
            row.Cells[1].AddParagraph("The shipowner declares that has taken full and clear notice of the Marina's Operational Rules, part of which is attached below (terms and conditions) and the current price list, on which the present agreement is based and which he expressly, unreservedly and irrevocably accepts.");
            return row;
        }

        private static Row SignatureHeaders(Section section) {
            var table = section.Footers.Primary.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(7);
            table.AddColumn("9.5cm");
            table.AddColumn("9.5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ο πελάτης / The client");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Εκ μέρους της Μαρίνας / On behalf of the Marina");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private Row Signatures(Section section) {
            var table = section.Footers.Primary.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(5);
            table.AddColumn("9.5cm");
            table.AddColumn("9.5cm");
            Row row = table.AddRow();
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
            var x = GetConnectedUsername();
            var z = "Terms and Conditions" + Path.DirectorySeparatorChar + x + ".jpg";
            if (File.Exists(z)) {
                row.Cells[1].AddParagraph().AddImage(z).Width = "4cm";
            } else {
                row.Cells[1].AddParagraph();
            }
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 0;
            row.Height = "2cm";
            return row;
        }

        private Row Username(Section section) {
            var table = section.Footers.Primary.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(7);
            table.AddColumn("9.5cm");
            table.AddColumn("9.5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[1].AddParagraph("Signed by user: " + GetConnectedUsername()).Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row Spacer(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0;
            table.AddColumn("19cm");
            var row = table.AddRow();
            return row;
        }

        private static void TermsAndConditions(Document document, Section section) {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Verdana";
            section.PageSetup.TopMargin = 40;
            section.AddPageBreak();
            Terms(section, "EN");
            section.AddPageBreak();
            Terms(section, "EL");
        }

        private static Row Terms(Section section, string language) {
            var table = section.AddTable();
            table.Borders.Width = 0;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(4.4);
            table.AddColumn("19cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Top;
            row.Cells[0].AddParagraph(OpenTermsFile(language)).Format.Alignment = ParagraphAlignment.Left;
            return row;
        }

        private static string OpenTermsFile(string language) {
            try {
                using StreamReader reader = new("Terms and Conditions" + Path.DirectorySeparatorChar + "Terms and Conditions " + language + ".txt");
                string text = reader.ReadToEndAsync().Result;
                return text;
            }
            catch (IOException e) {
                return e.InnerException.Message;
            }
        }

        private static string SavePdf(Document document, string reservationId) {
            var pdfRenderer = new PdfDocumentRenderer { Document = document };
            var filename = reservationId + ".pdf";
            pdfRenderer.RenderDocument();
            pdfRenderer.Save(Path.Combine("Reports" + Path.DirectorySeparatorChar + filename));
            return filename;
        }

        private string GetConnectedUsername() {
            return Identity.GetConnectedUserDetails(userManager, Identity.GetConnectedUserId(httpContextAccessor)).UserName;
        }

    }

}