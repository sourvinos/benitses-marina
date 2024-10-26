import { Guid } from 'guid-typescript'
// Custom
import { BerthWriteDto } from './berth-write-dto'
import { ReservationLeaseWriteDto } from './reservationLease-write-dto'
import { ReservationOwnerWriteDto } from './reservationOwner-write-dto'

export interface ReservationWriteDto {

    reservationId: Guid
    boatName: string
    loa: string
    beam: string
    draft: string
    fromDate: string
    toDate: string
    email: string
    remarks: string
    financialRemarks: string
    isDocked: boolean
    paymentStatusId: number
    isLongTerm: boolean
    isAthenian: boolean
    berths: BerthWriteDto[]
    reservationOwner: ReservationOwnerWriteDto
    reservationLease: ReservationLeaseWriteDto
    putAt: string

}
