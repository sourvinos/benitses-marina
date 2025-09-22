import { Guid } from 'guid-typescript'
// Custom
import { BerthWriteDto } from './berth-write-dto'
import { ReservationBoatWriteDto } from './reservation-boat-write-dto'
import { ReservationFeeDto } from './reservation-fee-dto'
import { ReservationInsuranceDto } from './reservation-insurance-dto'
import { ReservationPersonDto } from './reservation-person-dto'
import { ReservationFishingLicenceDto } from './reservation-fishing-licence-dto'

export interface ReservationWriteDto {

    reservationId: Guid
    fromDate: string
    toDate: string
    remarks: string
    financialRemarks: string
    isAthenian: boolean
    isDocked: boolean
    isDryDock: boolean
    isRequest: boolean
    paymentStatusId: number
    berths: BerthWriteDto[]
    boat: ReservationBoatWriteDto
    fishingLicence: ReservationFishingLicenceDto
    insurance: ReservationInsuranceDto
    owner: ReservationPersonDto
    billing: ReservationPersonDto
    fee: ReservationFeeDto
    putAt: string

}
