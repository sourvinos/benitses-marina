using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Features.Reservations;
using API.Infrastructure.Classes;
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

namespace API.Features.LeaseAgreements {

    public class LeaseAgreementRepository : Repository<Reservation>, ILeaseAgreementRepository {

        private readonly IMapper mapper;

        public LeaseAgreementRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<LeaseAgreementVM> GetByIdAsync(string reservationId) {
            var x = await context.Reservations
                .AsNoTracking()
                .Include(x => x.Boat)
                .Include(x => x.Insurance)
                .Include(x => x.Owner)
                .Include(x => x.Billing)
                .Include(x => x.Fee)
                .Where(x => x.ReservationId.ToString() == reservationId)
                .SingleOrDefaultAsync();
            return mapper.Map<Reservation, LeaseAgreementVM>(x);
        }

        public string BuildLeaseAgreement(LeaseAgreementVM leaseAgreement) {
            var document = new Document();
            Style style = document.Styles["Normal"];
            style.Font.Name = "Verdana";
            var section = document.AddSection();
            section.PageSetup.TopMargin = 40;
            LogoAndCompany(section);
            Spacer(section);
            Header(section);
            Spacer(section);
            VesselNameAndFlag(leaseAgreement.Boat, section);
            PortAndVesselRegistryNo(leaseAgreement.Boat, section);
            VesselDimensions(leaseAgreement.Boat, section);
            VesselTypeAndUse(leaseAgreement.Boat, section);
            MooringPeriod(leaseAgreement.Period, section);
            InsuranceCompany(leaseAgreement.Insurance, section);
            PolicyNoAndExpireDate(leaseAgreement.Insurance, section);
            Spacer(section);
            PersonHeaders(section);
            PersonNames(leaseAgreement.Owner, leaseAgreement.Billing, section);
            PersonAddresses(leaseAgreement.Owner, leaseAgreement.Billing, section);
            PersonVATs(leaseAgreement.Owner, leaseAgreement.Billing, section);
            PersonTaxOffices(leaseAgreement.Owner, leaseAgreement.Billing, section);
            PersonIdNumbers(leaseAgreement.Owner, leaseAgreement.Billing, section);
            PersonPhoneNumbers(leaseAgreement.Owner, leaseAgreement.Billing, section);
            PersonEmails(leaseAgreement.Owner, leaseAgreement.Billing, section);
            Spacer(section);
            CaptainHeaders(section);
            CaptainDetails(section);
            Spacer(section);
            FeeHeaders(section);
            FeeDetails(leaseAgreement.Fee, section);
            Spacer(section);
            SmallTerms(section);
            Spacer(section);
            Signatures(section);
            SignatureSpaces(section);
            TermsAndConditions(document, section);
            return SavePdf(document, leaseAgreement.ReservationId.ToString());
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
            table.AddColumn("5cm");
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
            table.AddColumn("16cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 7;
            row.BottomPadding = 7;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("ΣΥΜΒΑΣΗ ΠΑΡΑΜΟΝΗΣ ΣΚΑΦΟΥΣ\nVESSEL'S BERTH LEASE AGREEMENT");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row VesselNameAndFlag(LeaseAgreementBoatVM boat, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row PortAndVesselRegistryNo(LeaseAgreementBoatVM boat, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row VesselDimensions(LeaseAgreementBoatVM boat, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("2.34cm");
            table.AddColumn("3cm");
            table.AddColumn("2.34cm");
            table.AddColumn("3cm");
            table.AddColumn("2.32cm");
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

        private static Row VesselTypeAndUse(LeaseAgreementBoatVM boat, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row MooringPeriod(LeaseAgreementPeriodVM period, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row InsuranceCompany(LeaseAgreementInsuranceVM insurance, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("13cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ασφαλιστική εταιρία");
            row.Cells[0].AddParagraph("Insurance company");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(insurance.InsuranceCompany);
            return row;
        }

        private static Row PolicyNoAndExpireDate(LeaseAgreementInsuranceVM insurance, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Αρ. ασφαλιστηρίου");
            row.Cells[0].AddParagraph("Policy No");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph(insurance.PolicyNo);
            row.Cells[2].AddParagraph("Ημερ/νία λήξης");
            row.Cells[2].AddParagraph("Valid until");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph(DateHelpers.FormatDateStringToLocaleString(insurance.PolicyEnds));
            return row;
        }

        private static Row PersonHeaders(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("8cm");
            table.AddColumn("8cm");
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

        private static Row PersonNames(LeaseAgreementPersonVM owner, LeaseAgreementPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row PersonAddresses(LeaseAgreementPersonVM owner, LeaseAgreementPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row PersonVATs(LeaseAgreementPersonVM owner, LeaseAgreementPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row PersonTaxOffices(LeaseAgreementPersonVM owner, LeaseAgreementPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row PersonIdNumbers(LeaseAgreementPersonVM owner, LeaseAgreementPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row PersonPhoneNumbers(LeaseAgreementPersonVM owner, LeaseAgreementPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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

        private static Row PersonEmails(LeaseAgreementPersonVM owner, LeaseAgreementPersonVM billing, Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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
            table.AddColumn("16cm");
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
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
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
            table.AddColumn("16cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Οικονομικά στοιχεία / Fees and taxes");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row FeeDetails(LeaseAgreementFeeVM fee, Section section) {
            var locale = CultureInfo.CreateSpecificCulture("el-GR");
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromCentimeter(0.25);
            table.AddColumn("12cm");
            table.AddColumn("4cm");
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
            table.AddColumn("8cm");
            table.AddColumn("8cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ο πλοιοκτήτης δηλώνει ότι έλαβε γνώση όλων των όρων του ισχύοντος Ειδικού Κανονισμού Λειτουργίας της Μαρίνας (απόσπασμα του οποίου προσαρτάται στην παρούσα) και του ισχύοντος τιμοκαταλόγου, τους οποίους αποδέχεται ανεπιφύλακτα και οι οποίοι θα διέπουν την παρούσα.");
            row.Cells[1].AddParagraph("The shipowner declares that has taken full and clear notice of the Marina's Operational Rules, part of which is attached below (terms and conditions) and the current price list, on which the present agreement is based and which he expressly, unreservedly and irrevocably accepts.");
            return row;
        }

        private static Row Signatures(Section section) {
            var table = section.Footers.Primary.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(7);
            table.AddColumn("8cm");
            table.AddColumn("8cm");
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

        private static Row SignatureSpaces(Section section) {
            var table = section.Footers.Primary.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(7);
            table.AddColumn("8cm");
            table.AddColumn("8cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 50;
            return row;
        }

        private static Row Spacer(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0;
            table.AddColumn("16cm");
            var row = table.AddRow();
            return row;
        }

        private static void TermsAndConditions(Document document, Section section) {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Verdana";
            section.PageSetup.TopMargin = 40;
            section.AddPageBreak();
            TermsInEnglish(section);
        }

        private static Row TermsInEnglish(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(6.5);
            table.AddColumn("8cm");
            table.AddColumn("8cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Top;
            row.Cells[0].AddParagraph("GENERAL BERTHING REGULATIONS\n\n").Format.Alignment = ParagraphAlignment.Center; ;
            row.Cells[0].AddParagraph("The regulations which follow are excerpts from the Operational Regulations of the Marina.\n\nBerthing position and duration of berthing are determined by the marina. In case of absence of a yacht from its allocated berth the Marina has the right to moor another yacht in that berth.\n\nA berthing position may only be changed by prior agreement with the Marina.\n\nThe Marina may request that a yacht changes its berth or leaves the Marina: a) to enable the proper operation of the Marina, b) If there is a probability that the yacht will cause damage to another yacht or yachts, c) if the yacht is improperly or illegally occupying a particular berth.\n\nAnchoring anywhere in the Marina sea area is forbidden unless permission to do so has been granted by the Marina.\n\nAll yachts berthed in the Marina are obliged to have all necessary registration documentation issued by the Authorities of their origin country, as well as insurance cover.\n\nThe captain of any yacht is obliged to ensure that the yacht is safely moored. The Marina staff may check a yacht’s mooring and indicate changes if required. In the event of an emergency the Marina staff may assume all necessary actions to ensure safety to other Marina users.\n\nYachts entering the Marina sea area are obliged to communicate with the Marina by VHF and keep the max speed limit iaw the Marina’s Regulation.\n\nOn arrival the owner or captain is required to complete the forms given to them by the Marina staff and to present their ship’s paper.\n\nA yacht may not leave the Marina without prior notification to the Marina.\n\nBerthing fees are paid in advance upon the yacht’s arrival. Prior to a yacht’s departure berthing fees and any other outstanding charges must be paid in full.\n\nRefueling is only permitted at the Marina fuel station if any. If not, only by appropriate fuel trucks.\n\nThe Marina bears no responsibility for any damage caused to yachts due to, bad weather conditions or acts of God.\n\nRepair or maintenance works on yachts may be carried out by persons other than Marina staff only if prior permission has been granted by the Marina. In such a case a declaration must be made by the yacht owner that these persons are covered by the Greek State National Insurance System and that the yacht owner assumes liability for any third-party material damages or personal injury caused by these persons.\n\nIt is forbidden for any materials or objects to be ejected into the Marina sea area or to leave any materials or equipment on the Marina land area.\n\nThe use of lifting machines can only be undertaken by agreement with the Marina.\n\nUse of the Marina storage facilities can only be made by agreement with the Marina and only if items/materials to be stored are and suitably packaged and non- perishable.\n\nIn case of changing the yacht’s ownership the previous owner must notify the Marina the details of the new owner and the date of change of ownership.\n\nIt is explicitly agreed to use parking services free of charge for annual contracts. For contracts with a duration of 3-8 months, a 20% discount is provided. In any other case, the current price list is applied.\n\nAny legal disputes may only be resolved by the Greek local Courts.");
            row.Cells[1].AddParagraph("ΓΕΝΙΚΟΙ ΟΡΟΙ ΕΛΛΙΜΕΝΙΣΜΟΥ\n\n").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Οι όροι που ακολουθούν αποτελούν περίληψη από τον Κανονισμό Λειτουργίας της μαρίνας.\n\nΕλλιμενισμός επιτρέπεται μόνον στη θέση και γα το χρονικό διάστημα που υποδεικνύεται από την Μαρίνα. Σε περίπτωση απουσίας σκάφους από τη θέση του, η Μαρίνα έχει το δικαίωμα να ελλιμενίσει άλλο σκάφος στη  θέση αυτή.\n\nΑλλαγή θέσης επιτρέπεται μόνο κατόπιν αδείας της Μαρίνας.\n\nΜεθόρμιση σκάφους σε άλλη θέση ή και εκτός του λιμένα, αποφασίζεται από την Μαρίνα σε περίπτωση που  α) παρακωλύεται η λειτουργία του λιμένα, β) πιθανολογείται πρόκληση ζημιάς σε άλλο σκάφος  και γ) υπάρχει αυθαίρετη κατάληψη της θέσης ελλιμενισμού.\n\nΑγκυροβολία στο θαλάσσιο χώρο της Μαρίνας απαγορεύεται, εκτός αν δοθεί άδεια από την Μαρίνα.\n\nΤα ελλιμενιζόμενα σκάφη οφείλουν να είναι εφοδιασμένα με τα απαραίτητα διαπιστωτικά έγγραφα, καθώς και με ασφαλιστήριο συμβόλαιο.\n\nΟ κυβερνήτης κάθε σκάφους οφείλει να μεριμνά για την ασφαλή πρόσδεση του σκάφους. Το προσωπικό της Μαρίνας μπορεί να επιθεωρεί τον τρόπο πρόσδεσης του σκάφους να προβαίνει σε υποδείξεις αλλά και σε ενέργειες λήψης μέτρων ασφαλείας, σε περίπτωση έκτακτης ανάγκης.\n\nΚατά την προσέγγιση στη θαλάσσια ζώνη της Μαρίνας τα σκάφη οφείλουν να επικοινωνούν με την Μαρίνα στο VHF και να τηρούν το όριο ταχύτητας σύμφωνα με τον Κανονισμό της Μαρίνας.\n\nΑμέσως μετά τον κατάπλου οι κυβερνήτες των σκαφών οφείλουν να συμπληρώσουν τα έγγραφα που τους παραδίδονται και να επιδεικνύουν τα ναυτιλιακά έγγραφα του σκάφους. Πριν τον απόπλου του σκάφους ο κυβερνήτης του οφείλει ενημερώνει τη Μαρίνα.\n\nΤα τέλη ελλιμενισμού προκαταβάλλονται με τον κατάπλου του σκάφους. Πριν τον απόπλου θα πρέπει να έχουν εξοφληθεί τα τέλη ελλιμενισμού και τυχόν άλλες οφειλές προς την Μαρίνα.\n\nΕφοδιασμός Καυσίμων επιτρέπεται μόνον από τον Σταθμό Καυσίμων του Λιμένα εφόσον υπάρχει άλλως από βυτιοφόρα οχήματα που διαθέτουν τις απαραίτητες άδειες.\n\nΗ Μαρίνα δεν φέρει καμία ευθύνη για οποιαδήποτε ζημιά μπορεί να προκληθεί σε ελλιμενισμένα σκάφη εξαιτίας εξ αιτίας δυσμενών καιρικών φαινομένων ή συνθηκών ανωτέρας βίας.\n\nΕργασίες συντήρησης ή επισκευής σε σκάφη από πρόσωπα που δεν ανήκουν στο προσωπικό της Μαρίνας μπορεί να εκτελεσθούν  μόνο κατόπιν αδείας της Μαρίνας. Στην περίπτωση αυτή, απαιτείται δήλωση του ιδιοκτήτη ότι τα πρόσωπα, που θα εργασθούν στο σκάφος του, είναι ασφαλισμένα σε δημόσιο ασφαλιστικό φορέα  και ότι αναλαμβάνει την ευθύνη για οποιαδήποτε ζημιά προκληθεί σε πρόσωπα ή πράγματα από τους εργαζομένους αυτούς.\n\nΑπαγορεύεται η απόρριψη στη θάλασσα οποιουδήποτε υλικού ή αντικειμένου, καθώς και η εγκατάλειψη οποιουδήποτε υλικού ή εξοπλισμού στους χώρους του Λιμένα.\n\nΗ χρήση ανυψωτικών μηχανημάτων γίνεται μόνο σε συνεννόηση με την Μαρίνα.\n\nΗ χρήση αποθηκευτικών χώρων επιτρέπεται μετά από συνεννόηση με τη Μαρίνα και εφόσον τα προς αποθήκευση υλικά δεν είναι εύφλεκτα και είναι κατάλληλα συσκευασμένα.\n\nΣε περίπτωση  αλλαγής ιδιοκτησίας ελλιμενισμένου σκάφους, ο πωλητής οφείλει να γνωστοποιήσει εγγράφως στη Μαρίνα, τα στοιχεία του νέου ιδιοκτήτη και την ημερομηνία μεταβίβασης.\n\nΡητά συμφωνείται η δωρεάν χρήση υπηρεσιών στάθμευσης για ετήσια συμβόλαια. Για συμβόλαια με διάρκεια 3-8 μηνών παρέχεται έκπτωση 20%. Σε κάθε άλλη περίπτωση εφαρμόζεται ο ισχύων τιμοκατάλογος.\n\nΓια κάθε διαφορά από το παρόν, ορίζονται αρμόδια τα τοπικά δικαστήρια και εφαρμοστέο δίκαιο το Ελληνικό.");
            return row;
        }

        private static string SavePdf(Document document, string reservationId) {
            var pdfRenderer = new PdfDocumentRenderer { Document = document };
            var filename = reservationId + ".pdf";
            pdfRenderer.RenderDocument();
            pdfRenderer.Save(Path.Combine("Reports" + Path.DirectorySeparatorChar + filename));
            return filename;
        }

    }

}