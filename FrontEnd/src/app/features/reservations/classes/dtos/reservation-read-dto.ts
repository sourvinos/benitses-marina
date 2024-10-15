import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { BerthReadDto } from './berth-read-dto'

export interface ReservationReadDto extends Metadata {

    reservationId: Guid
    boatName: string
    customer: string
    loa: string
    fromDate: string
    toDate: string
    days: number
    email: string
    contact: string
    remarks: string
    isDocked: boolean
    paymentStatus: SimpleEntity
    isLongTerm: boolean
    berths: BerthReadDto[]
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
