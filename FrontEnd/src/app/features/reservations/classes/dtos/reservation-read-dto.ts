import { Guid } from 'guid-typescript'
// Custom
import { BerthReadDto } from './berth-read-dto'
import { Metadata } from 'src/app/shared/classes/metadata'
import { ReservationLeaseReadDto } from './reservationLease-read-dto'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ReservationReadDto extends Metadata {

    reservationId: Guid
    boatName: string
    loa: string
    fromDate: string
    toDate: string
    days: number
    email: string
    contact: string
    remarks: string
    financialRemarks: string
    isDocked: boolean
    paymentStatus: SimpleEntity
    isLongTerm: boolean
    isAthenian: boolean
    berths: BerthReadDto[]
    reservationLease: ReservationLeaseReadDto
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
