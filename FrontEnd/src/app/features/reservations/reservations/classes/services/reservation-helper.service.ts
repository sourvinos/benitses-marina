import { Injectable } from '@angular/core'
// Custom
import { ReservationStorage } from '../storage/reservation-storage'

@Injectable({ providedIn: 'root' })

export class ReservationHelperService {

    public createCachedReservation(form: any): ReservationStorage {
        var x = {
            reservationId: form.reservationId,
            boatName: form.boatName,
            flag: form.flag,
            loa: form.loa,
            beam: form.beam,
            draft: form.draft,
            registryPort: form.registryPort,
            registryNo: form.registryNo,
            boatType: form.boatType,
            boatUsage: form.boatUsage,
            fromDate: form.fromDate,
            toDate: form.toDate,
            berths: form.berths,
            remarks: form.remarks,
            financialRemarks: form.financialRemarks,
            paymentStatus: form.paymentStatus,
            insuranceCompany: form.insuranceCompany,
            policyNo: form.policyNo,
            policyEnds: form.policyEnds,
            netAmount: form.netAmount,
            vatPercent: form.vatPercent,
            vatAmount: form.vatAmount,
            grossAmount: form.grossAmount,
            isAthenian: form.isAthenian,
            isDocked: form.isDocked,
            isDryDock: form.isDryDock,
            isFishingBoat: form.isFishingBoat,
            isRequest: form.isRequest,
            isCash: form.isCash,
            ownerName: form.ownerName,
            ownerAddress: form.ownerAddress,
            ownerTaxNo: form.ownerTaxNo,
            ownerTaxOffice: form.ownerTaxOffice,
            ownerPassportNo: form.ownerPassportNo,
            ownerPhones: form.ownerPhones,
            ownerEmail: form.ownerEmail,
            billingName: form.billingName,
            billingAddress: form.billingAddress,
            billingTaxNo: form.billingTaxNo,
            billingTaxOffice: form.billingTaxOffice,
            billingPassportNo: form.billingPassportNo,
            billingPhones: form.billingPhones,
            billingEmail: form.billingEmail,
            postAt: form.postAt,
            postUser: form.postUser,
            putAt: form.putAt,
            putUser: form.putUser
        }
        return x
    }

}
