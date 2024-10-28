import { Guid } from 'guid-typescript'
// Custom
import { BerthWriteDto } from './berth-write-dto'
import { ReservationBoatDto } from './reservation-boat-dto'
import { ReservationFeeDto } from './reservation-fee-dto'
import { ReservationInsuranceDto } from './reservation-insurance-dto'
import { ReservationOwnerWriteDto } from './reservationOwner-write-dto'

export interface ReservationWriteDto {

    reservationId: Guid
    fromDate: string
    toDate: string
    remarks: string
    financialRemarks: string
    isDocked: boolean
    paymentStatusId: number
    isLongTerm: boolean
    isAthenian: boolean
    berths: BerthWriteDto[]
    boat: ReservationBoatDto
    insurance: ReservationInsuranceDto
    owner: ReservationOwnerWriteDto
    billing: ReservationOwnerWriteDto
    fee: ReservationFeeDto
    putAt: string

}
