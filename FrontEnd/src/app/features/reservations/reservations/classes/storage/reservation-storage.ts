import { Metadata } from 'src/app/shared/classes/metadata'
import { ReservationStorageSimpleEntity } from './reservation-storage-simple-entity'

export interface ReservationStorage extends Metadata {

    reservationId: string
    boatName: string
    flag: string
    loa: string,
    beam: string,
    draft: string
    registryPort: string
    registryNo: string
    boatType: ReservationStorageSimpleEntity
    boatUsage: ReservationStorageSimpleEntity
    fromDate: string
    toDate: string
    berths: [],
    remarks: string
    financialRemarks: string
    paymentStatus: ReservationStorageSimpleEntity
    insuranceCompany: string
    policyNo: string
    policyEnds: string
    netAmount: number
    discountPercent: number,
    discountAmount: number,
    netAmountAfterDiscount: number,
    vatPercent: number
    vatAmount: number
    grossAmount: number
    isAthenian: boolean
    isDocked: boolean
    isDryDock: boolean
    isFishingBoat: boolean
    isRequest: boolean
    isCash: boolean
    ownerName: string
    ownerAddress: string
    ownerTaxNo: string
    ownerTaxOffice: string
    ownerPassportNo: string
    ownerPhones: string
    ownerEmail: string
    billingName: string
    billingAddress: string
    billingTaxNo: string
    billingTaxOffice: string
    billingPassportNo: string
    billingPhones: string
    billingEmail: string
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
