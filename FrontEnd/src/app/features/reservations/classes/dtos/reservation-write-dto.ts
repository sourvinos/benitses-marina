import { Guid } from 'guid-typescript'
// Custom
import { BerthWriteDto } from './berth-write-dto'
import { ReservationBoatWriteDto } from './reservation-boat-write-dto'
import { ReservationFeeDto } from './reservation-fee-dto'
import { ReservationInsuranceDto } from './reservation-insurance-dto'
import { ReservationPersonDto } from './reservation-person-dto'

export interface ReservationWriteDto {

    reservationId: Guid
    fromDate: string
    toDate: string
    remarks: string
    financialRemarks: string
    isDocked: boolean
    paymentStatusId: number
    isDryDock: boolean
    isAthenian: boolean
    berths: BerthWriteDto[]
    boat: ReservationBoatWriteDto
    insurance: ReservationInsuranceDto
    owner: ReservationPersonDto
    billing: ReservationPersonDto
    fee: ReservationFeeDto
    putAt: string

}
