using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Features.Reservations.Transactions;
using API.Infrastructure.Account;
using API.Infrastructure.Helpers;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.EmailServices {

    public class EmailQueueService : BackgroundService {

        #region variables

        private readonly EnvironmentSettings environmentSettings;
        private readonly IEmailAccountSender emailAccountSender;
        private readonly IEmailQueueRepository emailQueueRepo;
        private readonly IEmailUserDetailsSender emailUserSender;
        private readonly IInvalidInsuranceEmailSender emailInvalidInsuranceSender;
        private readonly IReservationEmailSender emailReservationSender;
        private readonly IEndOfLeaseEmailSender emailEndOfLease;
        private readonly IReservationRepository reservationRepository;
        private readonly UserManager<UserExtended> userManager;

        #endregion

        public EmailQueueService(IEndOfLeaseEmailSender emailEndOfLease, IEmailAccountSender emailAccountSender, IEmailQueueRepository queueRepo, IEmailUserDetailsSender emailUserDetailsSender, IInvalidInsuranceEmailSender emailInvalidInsuranceSender, IOptions<EnvironmentSettings> environmentSettings, IReservationEmailSender emailReservationSender, IReservationRepository reservationRepository, UserManager<UserExtended> userManager) {
            this.emailAccountSender = emailAccountSender;
            this.emailEndOfLease = emailEndOfLease;
            this.emailInvalidInsuranceSender = emailInvalidInsuranceSender;
            this.emailQueueRepo = queueRepo;
            this.emailReservationSender = emailReservationSender;
            this.emailUserSender = emailUserDetailsSender;
            this.environmentSettings = environmentSettings.Value;
            this.reservationRepository = reservationRepository;
            this.userManager = userManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(value: environmentSettings.EmailSecondsDelay), stoppingToken);
                var x = await emailQueueRepo.GetFirstNotCompleted();
                if (x != null) {
                    if (x.Initiator == "ResetPassword") { SendResetPassword(x); }
                    if (x.Initiator == "UserDetails") { await SendUserDetailsAsync(x); }
                    if (x.Initiator == "Reservation") { await SendReservationAsync(x); }
                    if (x.Initiator == "InvalidInsurance") { await SendInvalidInsuranceAsync(x); }
                    if (x.Initiator == "EndOfLease") { await SendEndOfLeaseNoteAsync(x); }
                }
            }
        }

        private async void SendResetPassword(EmailQueue emailQueue) {
            var x = userManager.Users.Where(x => x.Id == emailQueue.EntityId.ToString()).FirstOrDefaultAsync().Result;
            if (x != null) {
                var response = emailAccountSender.EmailForgotPassword(x.UserName, x.Displayname, x.Email, environmentSettings.BaseUrl + "/#/resetPassword?email=" + x.Email + "&token=" + WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await userManager.GeneratePasswordResetTokenAsync(x))));
                if (response.Exception == null) {
                    emailQueue.IsSent = true;
                    emailQueueRepo.Update(emailQueue);
                }
            }
        }

        private async Task SendUserDetailsAsync(EmailQueue emailQueue) {
            var x = await userManager.Users.Where(x => x.Id == emailQueue.EntityId.ToString()).FirstOrDefaultAsync();
            if (x != null) {
                var response = emailUserSender.EmailUserDetails(x);
                if (response.Exception == null) {
                    emailQueue.IsSent = true;
                    emailQueueRepo.Update(emailQueue);
                }
            }
        }

        private async Task SendReservationAsync(EmailQueue emailQueue) {
            var reservation = await reservationRepository.GetByIdAsync(emailQueue.EntityId.ToString(), true);
            if (reservation != null) {
                if (emailReservationSender.SendReservationToEmail(emailQueue, reservation.Boat.Name, reservation.Owner.Email).Exception == null) {
                    emailQueue.IsSent = true;
                    emailQueueRepo.Update(emailQueue);
                }
            }
        }

        private async Task SendInvalidInsuranceAsync(EmailQueue emailQueue) {
            var reservation = await reservationRepository.GetByIdAsync(emailQueue.EntityId.ToString(), true);
            if (reservation != null) {
                if (emailInvalidInsuranceSender.SendInvalidInsuranceToEmail(emailQueue, reservation.Boat.Name, reservation.Owner.Email).Exception == null) {
                    emailQueue.IsSent = true;
                    emailQueueRepo.Update(emailQueue);
                }
            }
        }

        private async Task SendEndOfLeaseNoteAsync(EmailQueue emailQueue) {
            var reservation = await reservationRepository.GetByIdAsync(emailQueue.EntityId.ToString(), true);
            if (reservation != null) {
                if (emailEndOfLease.SendEndOfLeaseToEmail(emailQueue, reservation).Exception == null) {
                    emailQueue.IsSent = true;
                    emailQueueRepo.Update(emailQueue);
                }
            }
        }

    }

}