import { Guid } from 'guid-typescript'
// Custom
import { BerthReadDto } from './berth-read-dto'
import { Metadata } from 'src/app/shared/classes/metadata'
import { ReservationBoatDto } from './reservation-boat-dto'
import { ReservationFeeDto } from './reservation-fee-dto'
import { ReservationInsuranceDto } from './reservation-insurance-dto'
import { ReservationPersonDto } from './reservation-person-dto'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ReservationReadDto extends Metadata {

    reservationId: Guid
    fromDate: string
    toDate: string
    remarks: string
    financialRemarks: string
    paymentStatus: SimpleEntity
    isAthenian: boolean
    isDocked: boolean
    isDryDock: boolean
    isRequest: boolean
    berths: BerthReadDto[]
    boat: ReservationBoatDto
    insurance: ReservationInsuranceDto
    owner: ReservationPersonDto
    billing: ReservationPersonDto
    fee: ReservationFeeDto
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
